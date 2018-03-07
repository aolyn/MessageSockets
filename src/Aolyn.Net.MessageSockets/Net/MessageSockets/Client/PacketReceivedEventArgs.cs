using System;

namespace Aolyn.Net.MessageSockets.Client
{
	public class PacketReceivedEventArgs : EventArgs
	{
		public object Packet { get; set; }

		public PacketReceivedEventArgs(object packet)
		{
			Packet = packet;
		}
	}
}