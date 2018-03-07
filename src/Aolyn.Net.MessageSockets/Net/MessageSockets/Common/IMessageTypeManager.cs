using System;

namespace Aolyn.Net.MessageSockets.Common
{
	public interface IMessageTypeManager
	{
		Type GetMessageType(ushort messageType);
		ushort GetMessageType(Type messageType);
	}
}