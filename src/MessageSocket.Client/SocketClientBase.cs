using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MessageSocket.Net;

namespace MessageSocket.Client
{
	public abstract class SocketClientBase<TPacket>
	{
		private readonly PacketBufferManager<TPacket> _bufferManager;
		private readonly IMessageSerializer<TPacket> _serializer;
		private readonly string _host;
		private readonly int _port;
		private readonly SemaphoreSlim _connectLock = new SemaphoreSlim(1);

		private CancellationTokenSource _canncelSource;
		private bool _disposed;
		private bool _connected;
		private NetworkStream _stream;
		private TcpClient _client;

		public event EventHandler Closed;
		public event EventHandler Connected;

		protected SocketClientBase(string host, int port, IMessageSerializer<TPacket> serializer)
		{
			_host = host;
			_port = port;
			_bufferManager = new PacketBufferManager<TPacket>(serializer);
			_serializer = serializer;
		}

		public async Task StartAsync()
		{
			try
			{
				await _connectLock.WaitAsync();

				if (_connected)
					return;

				_client = new TcpClient();
				await _client.ConnectAsync(_host, _port);
				_disposed = false;
				_connected = true;
				try
				{
					Connected?.Invoke(this, new EventArgs());
				}
				catch (Exception)
				{
					//ignore
				}
				_stream = _client.GetStream();
				_canncelSource = new CancellationTokenSource();

				var unused1 = ReceiveRunner<TPacket>.RunReceiveAsync(_stream, _bufferManager, OnPacketsReceived,
					_canncelSource.Token);
				var unused2 = unused1.ContinueWith(tsk => Dispose());
			}
			finally
			{
				_connectLock.Release();
			}
		}

		protected abstract void OnPacketsReceived(object[] packets);

		public async Task SendAsync(TPacket packet)
		{
			if (_disposed)
				throw new ObjectDisposedException(GetType().Name);

			var bytes = _serializer.Serialize(packet);
			await SendAsync(bytes);
		}

		private async Task SendAsync(byte[] data)
		{
			if (_disposed)
				throw new ObjectDisposedException(GetType().Name);

			var lengthBytes = BitConverter.GetBytes(data.Length);
			await _stream.WriteAsync(lengthBytes, 0, lengthBytes.Length);
			await _stream.WriteAsync(data, 0, data.Length);
		}

		public void Dispose()
		{
			try
			{
				if (_disposed) return;

				_canncelSource.Cancel();
				Closed?.Invoke(this, new EventArgs());
				_disposed = true;
				_connected = false;
				_client?.Dispose();
			}
			finally
			{
				_connectLock.Release();
			}
		}
	}
}
