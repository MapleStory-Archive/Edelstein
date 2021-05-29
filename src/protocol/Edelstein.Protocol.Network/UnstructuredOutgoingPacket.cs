﻿using System.IO;
using System.Numerics;
using System.Text;
using System;

namespace Edelstein.Protocol.Network
{
    public class UnstructuredOutgoingPacket : IPacket, IPacketWriter
    {
        private static readonly Encoding StringEncoding = Encoding.ASCII;

        private readonly MemoryStream _stream;
        private readonly BinaryWriter _writer;

        public ReadOnlySpan<byte> Buffer => _stream.ToArray();
        public long Length => _stream.Length;

        public UnstructuredOutgoingPacket()
        {
            _stream = new MemoryStream();
            _writer = new BinaryWriter(_stream);
        }

        public UnstructuredOutgoingPacket(Enum operation) : this()
            => WriteShort(Convert.ToInt16(operation));

        public IPacketWriter WriteByte(byte value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketWriter WriteBool(bool value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketWriter WriteShort(short value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketWriter WriteUShort(ushort value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketWriter WriteInt(int value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketWriter WriteUInt(uint value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketWriter WriteLong(long value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketWriter WriteULong(ulong value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketWriter WriteString(string value, short? length = null)
        {
            value ??= string.Empty;

            if (length.HasValue)
            {
                if (value.Length > length) value = value.Substring(0, length.Value);
                WriteBytes(StringEncoding.GetBytes(value.PadRight(length.Value, '\0')));
            }
            else
            {
                WriteShort((short)value.Length);
                WriteBytes(StringEncoding.GetBytes(value));
            }

            return this;
        }

        public IPacketWriter WriteBytes(byte[] value)
        {
            _writer.Write(value);
            return this;
        }

        public IPacketWriter WriteVector2(Vector2 value)
        {
            WriteShort((short)value.X);
            WriteShort((short)value.Y);
            return this;
        }

        public IPacketWriter WriteDateTime(DateTime value)
        {
            WriteLong(value.ToFileTimeUtc());
            return this;
        }
    }
}