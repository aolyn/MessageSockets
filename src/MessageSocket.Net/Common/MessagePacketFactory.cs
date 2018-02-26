using System.IO;

namespace MessageSocket.Net.Common
{
	public class MessagePacketFactory : IPacketFactory<IMessage>
	{
		private readonly IMessageSerializer _messageSerializer;

		public MessagePacketFactory(IMessageSerializer messageSerializer)
		{
			_messageSerializer = messageSerializer;
		}

		public IMessage ReadPacket(Stream stream, int count)
		{
			var payload = _messageSerializer.Deseriaize(stream, count);
			return (IMessage)payload;
		}

		public byte[] Serialize(IMessage packet)
		{
			return _messageSerializer.Serialize(packet);
		}
	}
}
