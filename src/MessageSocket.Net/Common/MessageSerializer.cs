using System;
using System.IO;

namespace MessageSocket.Net.Common
{
	/// <summary>
	/// Protobuf MessageSerializer
	/// </summary>
	/// <typeparam name="TPacket"></typeparam>
	public class MessageSerializer<TPacket> : IMessageSerializer<TPacket>
	{
		private readonly IPacketTypeManager _typeManager;
		private readonly ISerializer _serializer;

		public MessageSerializer(IPacketTypeManager typeManager, ISerializer serializer)
		{
			_typeManager = typeManager;
			_serializer = serializer;
		}

		public TPacket Deseriaize(Stream stream, int count)
		{
			var lengthBytes = new byte[2];
			stream.Read(lengthBytes, 0, 2);
			var messageType = BitConverter.ToUInt16(lengthBytes, 0);
			var type = _typeManager.GetPacketType(messageType);
			var obj = (TPacket)_serializer.Deserialize(stream, type);

			return obj;
		}

		public byte[] Serialize(TPacket packet)
		{
			using (var ms = new MemoryStream())
			{
				var packetType = _typeManager.GetPacketType(packet.GetType());
				var messageTypeBytes = BitConverter.GetBytes(packetType);
				ms.Write(messageTypeBytes, 0, messageTypeBytes.Length);
				_serializer.Serialize(packet, ms);
				return ms.ToArray();
			}
		}
	}
}
