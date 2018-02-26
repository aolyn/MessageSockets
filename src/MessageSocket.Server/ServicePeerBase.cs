using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MessageSocket.Net;

namespace MessageSocket.Server
{
	public abstract class ServicePeerBase : IServicePeer
	{
		private readonly Stream _stream;
		private readonly PacketBufferManager _bufferManager;
		private readonly CancellationTokenSource _canncelSource = new CancellationTokenSource();
		private readonly IMessageSerializer _serializer;
		private bool _disposed;

		public event EventHandler Closed;

		protected ServicePeerBase(Stream stream, IMessageSerializer serializer)
		{
			_stream = stream;
			_bufferManager = new PacketBufferManager(serializer);
			_serializer = serializer;
		}

		public void Start()
		{
			var unused1 = ReceiveRunner.RunReceiveAsync(_stream, _bufferManager, OnPacketsReceived,
				_canncelSource.Token);
			var unused2 = unused1.ContinueWith(tsk => Dispose());
		}

		public virtual void OnPacketsReceived(object[] packets)
		{
			foreach (var packet in packets)
			{
				OnMessageReceived(packet);
			}
		}

		public virtual void OnMessageReceived(object packet)
		{
		}

		public async Task SendAsync(object packet)
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
			if (_disposed) return;
			_disposed = true;

			_canncelSource.Cancel();
			_stream.Dispose();
			Closed?.Invoke(this, new EventArgs());
		}
	}
}
