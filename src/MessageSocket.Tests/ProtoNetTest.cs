using System;
using System.IO;
using System.Net.Sockets;
using ProtoBuf.Meta;
using Xunit;

namespace MessageSocket.Tests
{
	public class ProtoNetTest
	{
		[Fact]
		public void Test1()
		{
			var obj = 2332;
			using (var ms = new MemoryStream())
			{
				RuntimeTypeModel.Default.Serialize(ms, obj);
				var result = ms.ToArray();
				ms.Position = 0;
				var dv = RuntimeTypeModel.Default.Deserialize(ms, null, typeof(int));
			}
		}

		public async void Test2()
		{
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//var info = socket.DuplicateAndClose(0);
			var bytes = new byte[] { 1, 2 };

			await socket.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
		}
	}
}
