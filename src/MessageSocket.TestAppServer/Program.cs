using System;
using System.IO;
using MessageSocket.Net;
using MessageSocket.Test.Message;

namespace MessageSocket.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			var msg = new MessagePacket
			{
				Id = 1,
				Content = "hello " + DateTime.Now,
				SentDate = DateTime.Now,
			};
			var bytes = ProtobufHelper.Serialize(msg);
			var dobj = ProtobufHelper.Deserialize<MessagePacket>(bytes);

			var ms = new MemoryStream();
			ms.Write(BitConverter.GetBytes(12345), 0, 4);
			ms.Write(bytes, 0, bytes.Length);
			ms.Position = 4;
			var dobj2 = ProtobufHelper.Deserialize<MessagePacket>(ms);

			new ServerTest().Test();

			Console.WriteLine("Hello World!");
		}
	}
}
