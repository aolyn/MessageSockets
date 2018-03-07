using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aolyn.IO;

namespace Aolyn.Net.MessageSockets.Common
{
	internal class PacketBufferManager<TPacket>
	{
		private const int MaxPacketLength = 1024 * 1024 * 1024;
		private readonly ArraySegmentStream _bufferStream = new ArraySegmentStream();
		private readonly IMessageSerializer<TPacket> _packetFactory;
		private List<ArraySegment<byte>> _datas = new List<ArraySegment<byte>>();

		public PacketBufferManager(IMessageSerializer<TPacket> packetFactory)
		{
			_packetFactory = packetFactory ?? throw new ArgumentNullException(nameof(packetFactory));
		}

		public object[] ReadPackets(byte[] data, int offset, int count)
		{
			var temp = _datas.ToList();
			temp.Add(new ArraySegment<byte>(data, offset, count));

			var totalCount = ArraySegmentStream.GetLeftCount(temp.ToArray(), 0, 0);
			if (totalCount < 4)
			{
				var currentBytes = data.Skip(offset).Take(count).ToArray();
				temp[temp.Count - 1] = new ArraySegment<byte>(currentBytes);
				_datas = temp;
				return null;
			}

			_bufferStream.Reset(temp.ToArray());
			var packets = new List<object>();
			while (true)
			{
				var lengthBytes = new byte[4];
				var savePosition = _bufferStream.Position;
				var readLength = _bufferStream.Read(lengthBytes, 0, 4);
				_bufferStream.Position = savePosition;
				if (readLength < 4)
				{
					var currentBytes = data
						.Skip(offset + _bufferStream.SegmentPosition)
						.Take(count - _bufferStream.SegmentPosition)
						.ToArray();
					temp = temp.Skip(_bufferStream.SegmentIndex).ToList();
					temp[temp.Count - 1] = new ArraySegment<byte>(currentBytes);
					_datas = temp;
					return packets.ToArray();
				}

				var packetLength = BitConverter.ToInt32(lengthBytes, 0);
				if (packetLength > MaxPacketLength)
				{
					throw new InvalidDataException("packet excced max length");
				}

				var leftCount = _bufferStream.Length - _bufferStream.Position;
				if (leftCount < packetLength + 4) //no enough bytes
				{
					var currentBytes = data
						.Skip(offset + _bufferStream.SegmentPosition)
						.Take(count - _bufferStream.SegmentPosition)
						.ToArray();
					temp = temp.Skip(_bufferStream.SegmentIndex).ToList();
					temp[temp.Count - 1] = new ArraySegment<byte>(currentBytes);
					_datas = temp;
					return packets.ToArray();
				}

				_bufferStream.Read(lengthBytes, 0, 4);
				var pb = _packetFactory.Deseriaize(_bufferStream, packetLength);
				packets.Add(pb);

				if (_bufferStream.Length == _bufferStream.Position) //all byte read
				{
					_datas.Clear();
					return packets.ToArray();
				}
				//var usedDataLength = _bufferStream.Position;
			}
		}
	}
}
