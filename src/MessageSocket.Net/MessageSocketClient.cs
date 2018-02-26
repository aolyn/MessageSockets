using System;
using System.Reflection;
using MessageSocket.Client;
using MessageSocket.Net.Common;

namespace MessageSocket.Net
{
	public class MessageSocketClient<TPacket> : SocketClient<TPacket>
	{
		public event EventHandler<PacketReceivedEventArgs> PacketReceived;

		public MessageSocketClient(string host, int port, Assembly messageAssembly)
			: this(host, port, new ProtobufMessageSerializer<TPacket>(new PacketTypeManager(messageAssembly)))
		{
		}

		public MessageSocketClient(string host, int port, IMessageSerializer<TPacket> packetFactory)
			: base(host, port, packetFactory)
		{
		}

		protected override void OnPacketsReceived(object[] packets)
		{
			foreach (var item in packets)
			{
				PacketReceived?.Invoke(this, new PacketReceivedEventArgs(item));
			}
		}
	}
}
