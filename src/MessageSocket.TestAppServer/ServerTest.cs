using System;
using System.IO;
using System.Net;
using Aolyn.Net.MessageSockets.Common;
using Aolyn.Net.MessageSockets.Protobuf;
using Aolyn.Net.MessageSockets.Server;
using MessageSocket.Test.Message;

namespace MessageSocket.TestAppServer
{
	public class ServerTest
	{
		public void Test()
		{
			var server = new SocketServer(new IPEndPoint(IPAddress.Any, 1001),
				stream => new TestServicePeer(stream));
			server.Start();

			Console.ReadLine();
		}

		public class TestServicePeer : ServicePeerBase<object>
		{
			private static readonly MessageSerializer<object> Serializer = new MessageSerializer<object>(
				new MessageTypeManager(typeof(MessagePacket).Assembly), new ProtobufSerializer());

			public TestServicePeer(Stream stream) : base(stream, Serializer)
			{
			}

			public override async void OnMessageReceived(object packet)
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