using System;

namespace MessageSocket.Net.Common
{
	public interface IPacketTypeManager
	{
		Type GetPacketType(ushort messageType);
		ushort GetPacketType(Type messageType);
	}
}