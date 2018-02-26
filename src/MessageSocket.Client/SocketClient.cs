using MessageSocket.Net;

namespace MessageSocket.Client
{
	public class SocketClient : SocketClientBase
	{
		public SocketClient(string host, int port, IMessageSerializer packetFactory)
			: base(host, port, packetFactory)
		{
		}

		protected override void OnPacketsReceived(object[] packets)
		{
		}
	}
}
