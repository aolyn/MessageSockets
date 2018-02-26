#define USEPROTOBUF

using System;
using System.IO;
using ProtoBuf;

#if USEPROTOBUF

#endif

namespace MessageSocket.Net
{
	public class ProtobufHelper
	{
		static ProtobufHelper()
		{
#if USEPROTOBUF
			//RuntimeTypeModel.Default.Add(typeof(IPEndPoint), false).SetSurrogate(typeof(IPEndPointSurrogate));
#endif
		}

		public static byte[] Serialize(object obj)
		{
#if USEPROTOBUF
			if (obj == null) return new byte[0];

			using (var ms = new MemoryStream())
			{
				Serialize(obj, ms);
				return ms.ToArray();
			}
#else
			return new byte[0];
#endif
		}

		public static object Deserialize<T>(byte[] bytes)
		{
			using (var ms = new MemoryStream(bytes))
			{
				return Deserialize(typeof(T), ms);
			}
		}

		public static void Serialize(object obj, Stream ms)
		{
			Serializer.Serialize(ms, obj);
		}

		//public static void Serialize(object obj, BinaryWriter writer)
		//{
		//	using (var ms = new BinaryWriterStream(writer))
		//	{
		//		Serializer.Serialize(ms, obj);
		//	}
		//}

		public static object Deserialize(Type type, Stream source)
		{
#if USEPROTOBUF
			return Serializer.Deserialize(type, source);
#else
			return null;
#endif
		}

		public static object Deserialize<T>(Stream source)
		{
#if USEPROTOBUF
			return Serializer.Deserialize(typeof(T), source);
#else
			return default(T);
#endif
		}
	}
}
