using System;
using MessageSocket.Net;
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
			var client = new MessageSocketClient<object>("127.0.0.1", 1001, typeof(MessagePacket).Assembly);
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
