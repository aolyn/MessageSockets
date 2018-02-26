using MessageSocket.Net;

namespace MessageSocket.Client
{
	public class SocketClient<TPacket> : SocketClientBase<TPacket>
	{
		public SocketClient(string host, int port, IMessageSerializer<TPacket> packetFactory)
			: base(host, port, packetFactory)
		{
		}

		protected override void OnPacketsReceived(object[] packets)
		{
		}
	}
}
