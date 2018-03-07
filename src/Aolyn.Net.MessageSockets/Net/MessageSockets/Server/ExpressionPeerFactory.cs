using System;
using System.IO;

namespace Aolyn.Net.MessageSockets.Server
{
	public class ExpressionPeerFactory : IServicePeerFactory
	{
		private readonly Func<Stream, IServicePeer> _factory;

		public ExpressionPeerFactory(Func<Stream, IServicePeer> factory)
		{
			_factory = factory;
		}

		public IServicePeer GetPeer(Stream stream)
		{
			return _factory(stream);
		}
	}
}
