using System.IO;

namespace Aolyn.Net.MessageSockets.Common
{
	/// <summary>
	/// abstract of MessageSerializer used to serialize or deserialize packet
	/// </summary>
	/// <typeparam name="TPacket"></typeparam>
	public interface IMessageSerializer<TPacket>
	{
		/// <summary>
		/// deserialize packet from stream
		/// </summary>
		/// <param name="stream">source stream</param>
		/// <param name="count">data count</param>
		/// <returns></returns>
		TPacket Deseriaize(Stream stream, int count);

		/// <summary>
		/// serialize packet to bytes
		/// </summary>
		/// <param name="packet"></param>
		/// <returns></returns>
		byte[] Serialize(TPacket packet);
	}
}