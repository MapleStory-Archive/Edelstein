﻿using System;
using System.Collections.Generic;
using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Ciphers;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Pipeline
{
    public class NettyPacketDecoder : ReplayingDecoder<NettyPacketState>
    {
        private readonly ITransport _transport;

        private readonly AESCipher _aesCipher;
        private readonly IGCipher _igCipher;

        private short _sequence;
        private short _length;

        public NettyPacketDecoder(
            ITransport transport,
            AESCipher aesCipher,
            IGCipher igCipher
        ) : base(NettyPacketState.DecodingHeader)
        {
            _transport = transport;
            _aesCipher = aesCipher;
            _igCipher = igCipher;
        }

        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            var socket = context.Channel.GetAttribute(NettyAttributes.SocketKey).Get();

            try
            {
                switch (State)
                {
                    case NettyPacketState.DecodingHeader:
                        if (socket != null)
                        {
                            var sequence = input.ReadShortLE();
                            var length = input.ReadShortLE();

                            if (socket.EncryptData) length ^= sequence;

                            _sequence = sequence;
                            _length = length;
                        }
                        else _length = input.ReadShortLE();

                        Checkpoint(NettyPacketState.DecodingPayload);
                        return;
                    case NettyPacketState.DecodingPayload:
                        var buffer = new byte[_length];

                        input.ReadBytes(buffer);
                        Checkpoint(NettyPacketState.DecodingHeader);

                        if (_length < 0x2) return;

                        if (socket != null)
                        {
                            var seqRecv = socket.SeqRecv;
                            var version = (short)(seqRecv >> 16) ^ _sequence;

                            if (!(version == -(_transport.Version + 1) ||
                                  version == _transport.Version)) return;

                            if (socket.EncryptData)
                            {
                                _aesCipher.Transform(buffer, seqRecv);
                                ShandaCipher.DecryptTransform(buffer);
                            }

                            socket.SeqRecv = _igCipher.Hash(seqRecv, 4, 0);
                        }

                        output.Add(new UnstructuredIncomingPacket(buffer));
                        return;
                }
            }
            catch (IndexOutOfRangeException)
            {
                RequestReplay();
            }
        }
    }
}