using System.IO;

namespace MessageSocket.Net
{
	public interface IPacketFactory<TPacket>
	{
		TPacket ReadPacket(Stream stream, int count);
		byte[] Serialize(TPacket packet);
	}
}
