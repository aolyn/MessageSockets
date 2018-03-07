using System;
using Aolyn.Net.MessageSockets.Client;
using Aolyn.Net.MessageSockets.Common;
using Aolyn.Net.MessageSockets.Protobuf;
using MessageSocket.Test.Message;

namespace MessageSocket.TestAppClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Client");
			TestAsync().Wait();

			Console.WriteLine("Hello World!");
		}

		private static async System.Threading.Tasks.Task TestAsync()
		{
			var serializer1 = new MessageSerializer<object>(new MessageTypeManager(typeof(MessagePacket).Assembly),
				new ProtobufSerializer());

			var serializer = new MessageSerializer<object>(
				new MessageTypeManager(typeof(MessagePacket).Assembly), new ProtobufSerializer());

			var client = new SocketClient<object>("127.0.0.1", 1001, serializer);
			client.PacketReceived += (s, p) =>
			{
				Console.WriteLine($"message packet received {p.Packet}");
			};

			client.Closed += (s, p) =>
			{
				Console.WriteLine("client closed");
			};

			client.Connected += (s, p) =>
			{
				Console.WriteLine("client Connected");
			};

			await client.StartAsync();

			for (var i = 0; i < 9; i++)
			{
				Console.WriteLine("press any key to send");
				Console.ReadLine();

				var date = new DateTime(2017, 1, 2);
				await client.SendAsync(new MessagePacket
				{
					Id = 1,
					Content = "hello " + date,
					SentDate = date,
				});
			}

			Console.ReadLine();
		}
	}
}
