using System;

namespace MessageSocket.Net
{
	public interface IServicePeer : IDisposable
	{
		event EventHandler Closed;
		void Start();
	}
}