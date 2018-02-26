using System;
using System.Reflection;
using MessageSocket.Client;
using MessageSocket.Net.Common;

namespace MessageSocket.Net
{
	public class MessageSocketClient : SocketClient<IMessage>
	{
		public event EventHandler<PacketReceivedEventArgs<IMessage>> PacketReceived;

		public MessageSocketClient(string host, int port, Assembly messageAssembly)
			: base(host, port, new MessagePacketFactory(new ProtobufMessageSerializer(
				new PacketTypeManager(messageAssembly))))
		{
		}

		protected override void OnPacketsReceived(IMessage[] packets)
		{
			foreach (var item in packets)
			{
				PacketReceived?.Invoke(this, new PacketReceivedEventArgs<IMessage>(item));
			}
		}
	}
}
