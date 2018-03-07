using System;

namespace Aolyn.Net.MessageSockets.Common
{
	public class MessageAttribute : Attribute
	{
		public ushort Type { get; set; }
	}
}
