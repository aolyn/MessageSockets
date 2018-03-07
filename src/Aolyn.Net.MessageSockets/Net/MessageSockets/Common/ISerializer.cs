using System;
using System.IO;

namespace Aolyn.Net.MessageSockets.Common
{
	public interface ISerializer
	{
		object Deserialize(Stream stream, Type type);
		void Serialize(object obj, Stream stream);
	}
}
