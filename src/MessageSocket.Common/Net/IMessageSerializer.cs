using System.IO;

namespace MessageSocket.Net
{
	public interface IMessageSerializer
	{
		object Deseriaize(Stream stream, int count);
		byte[] Serialize(object packet);
	}
}