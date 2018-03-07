using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Aolyn.Net.MessageSockets.Server
{
	public class SocketServer : TcpServerBase
	{
		private readonly IServicePeerFactory _factory;
		private readonly List<IServicePeer> _peers = new List<IServicePeer>();

		private bool _isDisposed;

		public SocketServer(IPEndPoint listenEndPoint, IServicePeerFactory factory) : base(listenEndPoint)
		{
			_factory = factory;
		}

		public SocketServer(IPEndPoint listenEndPoint, Func<Stream, IServicePeer> factory) : base(listenEndPoint)
		{
			_factory = new ExpressionPeerFactory(factory);
		}

		protected override IServicePeer GetServerPeer(Stream stream)
		{
			var servicePeer = _factory.GetPeer(stream);
			_peers.Add(servicePeer);
			servicePeer.Closed += ServicePeer_Closed;
			return servicePeer;
		}

		private void ServicePeer_Closed(object sender, EventArgs e)
		{
			_peers.Remove((IServicePeer)sender);
		}

		public override void Dispose()
		{
			if (_isDisposed) return;
			_isDisposed = true;

			base.Dispose();
		}
	}
}
