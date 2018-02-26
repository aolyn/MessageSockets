using System;

namespace MessageSocket.Net.Common
{
	public class MessageAttribute : Attribute
	{
		public ushort Type { get; set; }
	}
}
