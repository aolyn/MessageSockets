//using System.IO;

//namespace MessageSocket.Net
//{
//	public class BytePacketFactory : IPacketFactory<byte[]>
//	{
//		public byte[] ReadPacket(Stream stream, int count)
//		{
//			var bytes = new byte[count];
//			var unused = stream.Read(bytes, 0, count);
//			return bytes;
//		}

//		public byte[] Serialize(byte[] packet)
//		{
//			return packet;
//		}
//	}
//}
