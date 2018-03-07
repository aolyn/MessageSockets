using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Aolyn.Net.MessageSockets.Server;
using log4net;

namespace Aolyn.Net.MessageSockets.Common
{
	public class MessageTypeManager : IMessageTypeManager
	{
		private readonly ILog _log = LogManager.GetLogger(typeof(TcpServerBase));

		//private static readonly Dictionary<PacketType, Func<object>> CreateHandlers;
		private static readonly Dictionary<ushort, Type> PacketTypeMappings = new Dictionary<ushort, Type>();
		private static readonly Dictionary<Type, ushort> TypePacketTypeMappings = new Dictionary<Type, ushort>();

		public MessageTypeManager(Assembly assembly) : this(new[] { assembly }, GetMessageTypeFromMessageAttribute)
		{
		}

		public MessageTypeManager(IEnumerable<Assembly> assmblies, Func<Type, ushort> getMessageTypeFunc)
		{
			foreach (var item in assmblies)
			{
				var types = item.GetTypes();

				//CreateHandlers = new Dictionary<PacketType, Func<object>>();
				foreach (var type in types)
				{
					if (type.IsAbstract) continue;

					var messageType = getMessageTypeFunc(type);
					if (messageType == 0) continue;

					try
					{
						//var ff = Expression.New(type);
						//var func = Expression.Lambda<Func<object>>(ff).Compile();
						//CreateHandlers.Add(attribute.Type, func);
						PacketTypeMappings[messageType] = type;
						TypePacketTypeMappings[type] = messageType;
					}
					catch (Exception ex)
					{
						//log
						//throw;
						_log.Debug(ex);
					}
				}

			}
		}

		private static ushort GetMessageTypeFromMessageAttribute(Type type)
		{
			var attributes = type.GetCustomAttributes<MessageAttribute>();
			var attribute = attributes.FirstOrDefault();
			if (attribute == null) return 0;
			var attributeType = attribute.Type;
			return attributeType;
		}

		public Type GetMessageType(ushort messageType)
		{
			return PacketTypeMappings.TryGetValue(messageType, out var value) ? value : null;
		}

		public ushort GetMessageType(Type type)
		{
			return TypePacketTypeMappings.TryGetValue(type, out var messageType)
				? messageType
				: (ushort)0;
		}

		//public object CreatePacket(PacketType messageType)
		//{
		//	if (!CreateHandlers.TryGetValue(messageType, out var handler))
		//		throw new InvalidOperationException("unkonw PacketType " + messageType);
		//	try
		//	{
		//		var packet = handler();
		//		return packet;
		//	}
		//	catch (Exception ex)
		//	{
		//		throw new Exception("Parse packet error.", ex);
		//	}
		//}
	}
}
