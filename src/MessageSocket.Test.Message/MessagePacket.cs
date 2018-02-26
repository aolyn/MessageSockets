using System;
using MessageSocket.Net.Common;
using ProtoBuf;

namespace MessageSocket.Test.Message
{
	[ProtoContract]
	[Message(Type = (ushort)PacketType.Message)]
	public class MessagePacket
	{
		[ProtoMember(1)]
		public int Id { get; set; }

		[ProtoMember(2)]
		public DateTime SentDate { get; set; }

		[ProtoMember(3)]
		public string Content { get; set; }
	}
}
