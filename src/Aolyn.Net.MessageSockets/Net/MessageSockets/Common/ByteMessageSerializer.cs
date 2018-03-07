using System.IO;

namespace Aolyn.Net.MessageSockets.Common
{
	public class ByteMessageSerializer : IMessageSerializer<byte[]>
	{
		public byte[] Deseriaize(Stream stream, int count)
		{
			var bytes = new byte[count];
			var unused = stream.Read(bytes, 0, count);
			return bytes;
		}

		public byte[] Serialize(byte[] packet)
		{
			return packet;
		}
	}
}
