using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MessageSocket.Net
{
	public abstract class TcpServerBase : IDisposable
	{
		private readonly List<IServicePeer> _peers = new List<IServicePeer>();
		private readonly TcpListener _listener;
		// ReSharper disable once NotAccessedField.Local
		private Task _runTask;
		private volatile bool _isRunCanceled;
		private bool _isDisposed;

		public IPEndPoint LocalPoint { get; protected set; }

		internal ServerStatus Status { get; private set; }

		protected TcpServerBase(IPEndPoint listenEndPoint)
		{
			LocalPoint = listenEndPoint;
			_listener = new TcpListener(listenEndPoint);
			_listener.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			Status = ServerStatus.Unstart;
		}

		public void Start()
		{
			_runTask = RunAsync();
			Status = ServerStatus.Running;
		}

		private async Task RunAsync()
		{
			try
			{
				_listener.Start();
				//Logger.Info($"ServerBase start listen on {_listener.LocalEndpoint}");
			}
			catch (Exception ex)
			{
				//Logger.Info($"ServerBase start listen on {_listener.LocalEndpoint} error " + ex);
				throw;
			}

			try
			{
				while (true)
				{
					if (_isRunCanceled) break;

					var socket = await _listener.AcceptSocketAsync().ConfigureAwait(false);

					//Logger.Debug($"Client Connected {socket.RemoteEndPoint}");

					if (_isRunCanceled) break;

					try
					{
						await StartPeerAsync(socket);
					}
					catch (Exception ex)
					{
						//Logger.Debug($"StartPeer Error: {ex}");
					}
				}
			}
			catch (SocketException ex)
			{
				//Logger.Debug($"Run Listener Error: SocketErrorCode {ex.SocketErrorCode}, {ex}");
			}
			catch (Exception ex)
			{
				//Logger.Debug($"Run Listener Error: {ex}");
			}
			finally
			{
				_listener.Stop();
				Dispose();
			}
		}

		private async Task StartPeerAsync(Socket socket)
		{
			var nets = new NetworkStream(socket, true);
			Stream destSteam = null;
			try
			{
				destSteam = await GetStreamAsync(nets);

				try
				{
					var nip = GetServerPeer(destSteam);
					_peers.Add(nip);
					nip.Closed += nip_OnPeerClosed;
					nip.Start();
				}
				catch (Exception ex)
				{
					//Logger.Default.Error(ex.ToString());
					destSteam.Dispose();
				}
			}
			catch (IOException ex)
			{
				//Logger.Debug($"StartPeer failed: " + ex);
				nets.Dispose();
				destSteam?.Dispose();
			}
		}

		protected virtual Task<Stream> GetStreamAsync(Stream stream)
		{
			return Task.FromResult(stream);
		}

		protected abstract IServicePeer GetServerPeer(Stream stream);

		private void nip_OnPeerClosed(object sender, EventArgs args)
		{
			var servicePeer = (IServicePeer)sender;
			servicePeer.Closed -= nip_OnPeerClosed;
			_peers.Remove(servicePeer);
		}

		//public void Pause()
		//{
		//	Status = ServerStatus.Paused;
		//}

		public virtual void Dispose()
		{
			if (_isDisposed) return;
			_isDisposed = true;

			Status = ServerStatus.Closed;

			_isRunCanceled = true;
			for (var i = 0; i < _peers.Count; i++)
			{
				_peers[i].Dispose();
			}

			_peers.Clear();
		}
	}
}