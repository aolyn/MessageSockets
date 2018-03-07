using System;

namespace Aolyn.Net.MessageSockets.Server
{
	public interface IServicePeer : IDisposable
	{
		event EventHandler Closed;
		void Start();
	}
}