using System.IO;
using MessageSocket.Net;

namespace MessageSocket.Server
{
	public interface IPeerFactory
	{
		IServicePeer GetPeer(Stream stream);
	}
}
