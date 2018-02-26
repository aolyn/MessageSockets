using System;
using System.IO;
using System.Net;
using MessageSocket.Net;
using MessageSocket.Net.Common;
using MessageSocket.Server;
using MessageSocket.Test.Message;

namespace MessageSocket.TestApp
{
	public class ServerTest
	{
		public void Test()
		{
			var server = new SocketServer(new IPEndPoint(IPAddress.Any, 1001),
				stream => new TestPeerService(stream));
			server.Start();

			Console.ReadLine();
		}

		public class TestPeerService : MessagePeerService
		{
			public TestPeerService(Stream stream) : base(stream, typeof(MessagePacket).Assembly)
			{
			}

			public override async void OnMessageReceived(IMessage packet)
			{
				if (packet is MessagePacket messagePacket)
				{
					Console.WriteLine($"message packet received {messagePacket.Content}");
					await SendAsync(new MessagePacket
					{
						Id = 1,
						Content = "hello " + messagePacket.SentDate,
						SentDate = messagePacket.SentDate,
					});
				}
			}
		}
	}
}
