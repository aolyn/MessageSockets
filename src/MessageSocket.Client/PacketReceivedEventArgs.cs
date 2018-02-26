using System;

namespace MessageSocket.Client
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