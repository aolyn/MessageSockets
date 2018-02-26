using System;
using System.Reflection;
using MessageSocket.Client;
using MessageSocket.Net.Common;

namespace MessageSocket.Net
{
	public class MessageSocketClient : SocketClient
	{
		public event EventHandler<PacketReceivedEventArgs> PacketReceived;

		public MessageSocketClient(string host, int port, Assembly messageAssembly)
			: base(host, port, new ProtobufMessageSerializer(new PacketTypeManager(messageAssembly)))
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
