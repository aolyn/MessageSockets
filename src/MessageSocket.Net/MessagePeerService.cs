using System.IO;
using System.Reflection;
using MessageSocket.Net.Common;
using MessageSocket.Server;

namespace MessageSocket.Net
{
	public class MessagePeerService : ServicePeerBase<IMessage>
	{
		public MessagePeerService(Stream stream, Assembly messageAssembly)
			: base(stream, new MessagePacketFactory(new ProtobufMessageSerializer(
				new PacketTypeManager(messageAssembly))))
		{
		}

		public override void OnMessageReceived(IMessage packet)
		{
		}
	}
}