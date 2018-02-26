using System;
using System.IO;
using MessageSocket.Net;

namespace MessageSocket.Server
{
	public class ExpressionPeerFactory : IPeerFactory
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
