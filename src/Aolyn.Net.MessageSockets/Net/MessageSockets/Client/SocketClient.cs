using System;
using Aolyn.Net.MessageSockets.Common;

namespace Aolyn.Net.MessageSockets.Client
{
	public class SocketClient<TPacket> : SocketClientBase<TPacket>
	{
		public event EventHandler<PacketReceivedEventArgs> PacketReceived;

		public SocketClient(string host, int port, IMessageSerializer<TPacket> serializer)
			: base(host, port, serializer)
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
