using System;
using System.IO;
using Aolyn.Net.MessageSockets.Common;
using ProtoBuf;

namespace Aolyn.Net.MessageSockets.Protobuf
{
	/// <inheritdoc />
	public class ProtobufSerializer : ISerializer
	{
		public object Deserialize(Stream source, Type type)
		{
			return Serializer.Deserialize(type, source);
		}

		public void Serialize(object instance, Stream destination)
		{
			Serializer.Serialize(destination, instance);
		}
	}
}
