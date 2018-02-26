using System.IO;

namespace MessageSocket.Net.Common
{
	public class ByteMessageSerializer : IMessageSerializer
	{
		public object Deseriaize(Stream stream, int count)
		{
			var bytes = new byte[count];
			var unused = stream.Read(bytes, 0, count);
			return bytes;
		}

		public byte[] Serialize(object packet)
		{
			return (byte[])packet;
		}
	}
}
