# MessageSockets

MessageSockets is a Tcp commnunication library. You can send & receive Strong Type object data between Server & Client. Tcp packet split is enabled by default, you can customize MessageSerialier which use to Serialize/Deserialize object and bytes data, in this libray Protobuf Serializer provided.

## Get Start

### 1. Define your message types

``` csharp
using System;
using Aolyn.Net.MessageSockets.Common;
using ProtoBuf;

namespace MessageSocket.Test.Message
{
    [ProtoContract]
	[Message(Type = (ushort)PacketType.Message)]
	public class MessagePacket
	{
		[ProtoMember(1)]
		public int Id { get; set; }

		[ProtoMember(2)]
		public DateTime SentDate { get; set; }

		[ProtoMember(3)]
		public string Content { get; set; }
	}
}

```

### 2. Create server

``` csharp
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
```

### 3. Create client
```csharp
using System;
using System.Threading.Tasks;
using Aolyn.Net.MessageSockets.Client;
using Aolyn.Net.MessageSockets.Common;
using Aolyn.Net.MessageSockets.Protobuf;
using MessageSocket.Test.Message;

namespace MessageSocket.TestAppClient
{
    internal class ClientTest
	{
		public static async Task TestAsync()
		{
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

```
