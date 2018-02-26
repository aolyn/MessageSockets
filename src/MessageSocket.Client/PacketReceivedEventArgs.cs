using System;

namespace MessageSocket.Client
{
	public class PacketReceivedEventArgs<TPacket> : EventArgs
	{
		public TPacket Packet { get; set; }

		public PacketReceivedEventArgs(TPacket packet)
		{
			Packet = packet;
		}
	}
}