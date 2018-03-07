using System.IO;

namespace Aolyn.Net.MessageSockets.Server
{
	public interface IServicePeerFactory
	{
		IServicePeer GetPeer(Stream stream);
	}
}
