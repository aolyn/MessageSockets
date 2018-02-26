using System.IO;
using System.Reflection;
using MessageSocket.Net.Common;
using MessageSocket.Server;

namespace MessageSocket.Net
{
	public class MessageServicePeer<TPacket> : ServicePeerBase<TPacket>
	{
		public MessageServicePeer(Stream stream, Assembly messageAssembly)
			: this(stream, new ProtobufMessageSerializer<TPacket>(new PacketTypeManager(messageAssembly)))
		{
		}

		protected MessageServicePeer(Stream stream, IMessageSerializer<TPacket> serializer) : base(stream, serializer)
		{
		}

		public override void OnMessageReceived(object packet)
		{
		}
	}
}