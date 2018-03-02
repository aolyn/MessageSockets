using System;
using System.IO;
using MessageSocket.Net.Common;
using ProtoBuf;

namespace MessageSocket.Protobuf
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
