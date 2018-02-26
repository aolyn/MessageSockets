using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MessageSocket.Net;

namespace MessageSocket
{
	public class ReceiveRunner
	{
		public static async Task RunReceiveAsync(Stream ss, PacketBufferManager bufferManager,
			Action<object[]> onPacketsReceived, CancellationToken cannellationToken)
		{
			const int readBufferSize = 1024;
			var readBuffer = new byte[readBufferSize];
			var readOffset = 0;
			while (true)
			{
				try
				{
					var rec = await ss.ReadAsync(readBuffer, readOffset, readBufferSize - readOffset,
						cannellationToken).ConfigureAwait(false);

					try
					{
						if (rec > 0)
						{
							var dataLength = readOffset + rec;
							if (dataLength > 10) //headeer length
							{
								var packets = bufferManager.ReadPackets(readBuffer, 0, dataLength);
								onPacketsReceived(packets);
								readOffset = 0;
							}
							else
							{
								readOffset += rec;
							}

							continue;
						}
					}
					catch (Exception)
					{
						//Logger.Default.Error(ex.ToString());
					}
				}
				catch (TaskCanceledException)
				{
				}
				catch (ObjectDisposedException)
				{
				}
				catch (IOException ex)
				{
					if (ex.InnerException is SocketException)
					{
						//Logger.Default.Debug($"TcpPeerService.RunReceive error, " +
						//$"SocketErrorCode: {socketException.SocketErrorCode}, {socketException.Message}");
					}
					else
					{
#if DEBUG_LOG_ALL
						Logger.Default.Error(ex.ToString());
#endif
						ex.Data["StackTrace"] = Environment.StackTrace;
						//throw;
					}
				}
				catch (Exception)
				{
					//Logger.Default.Error(ex.ToString());
					//throw;
				}

				break;
			}
		}

	}
}