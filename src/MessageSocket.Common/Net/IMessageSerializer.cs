using System.IO;

namespace MessageSocket.Net
{
	public interface IMessageSerializer<TPacket>
	{
		TPacket Deseriaize(Stream stream, int count);
		byte[] Serialize(TPacket packet);
	}
}