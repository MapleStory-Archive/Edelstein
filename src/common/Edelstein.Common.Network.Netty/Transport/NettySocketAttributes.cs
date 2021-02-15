﻿using DotNetty.Common.Utilities;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.Netty.Transport
{
    public class NettySocketAttributes
    {
        public static readonly AttributeKey<ISocket> SocketKey = AttributeKey<ISocket>.ValueOf("Socket");
        public static readonly AttributeKey<ISocketHandler> HandlerKey = AttributeKey<ISocketHandler>.ValueOf("Handler");
    }
}