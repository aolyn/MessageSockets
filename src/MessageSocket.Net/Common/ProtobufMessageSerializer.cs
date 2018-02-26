using System;
using System.IO;
using ProtoBuf;

namespace MessageSocket.Net.Common
{
	public class ProtobufMessageSerializer : IMessageSerializer
	{
		private readonly IPacketTypeManager _typeManager;

		public ProtobufMessageSerializer(IPacketTypeManager typeManager)
		{
			_typeManager = typeManager;
		}

		public object Deseriaize(Stream stream, int count)
		{
			var lengthBytes = new byte[2];
			stream.Read(lengthBytes, 0, 2);
			var messageType = BitConverter.ToUInt16(lengthBytes, 0);
			var type = _typeManager.GetPacketType(messageType);
			var obj = Serializer.Deserialize(type, stream);
			return obj;
		}

		public byte[] Serialize(object packet)
		{
			using (var ms = new MemoryStream())
			{
				var packetType = _typeManager.GetPacketType(packet.GetType());
				var messageTypeBytes = BitConverter.GetBytes(packetType);
				ms.Write(messageTypeBytes, 0, messageTypeBytes.Length);
				Serializer.Serialize(ms, packet);
				return ms.ToArray();
			}
		}
	}
}
