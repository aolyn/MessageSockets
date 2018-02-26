using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MessageSocket.Net.Common
{
	public class PacketTypeManager : IPacketTypeManager
	{
		//private static readonly Dictionary<PacketType, Func<object>> CreateHandlers;
		private static readonly Dictionary<ushort, Type> PacketTypeMappings = new Dictionary<ushort, Type>();
		private static readonly Dictionary<Type, ushort> TypePacketTypeMappings = new Dictionary<Type, ushort>();

		public PacketTypeManager(Assembly asm)
		{
			var types = asm.GetTypes();

			//CreateHandlers = new Dictionary<PacketType, Func<object>>();
			foreach (var type in types)
			{
				if (type.IsAbstract) continue;

				var attributes = type.GetCustomAttributes<MessageAttribute>();
				var attribute = attributes.FirstOrDefault();
				if (attribute == null) continue;

				try
				{
					//var ff = Expression.New(type);
					//var func = Expression.Lambda<Func<object>>(ff).Compile();
					//CreateHandlers.Add(attribute.Type, func);
					PacketTypeMappings[attribute.Type] = type;
					TypePacketTypeMappings[type] = attribute.Type;
				}
				catch (Exception)
				{
					//log
					//throw;
				}
			}
		}

		public Type GetPacketType(ushort messageType)
		{
			return PacketTypeMappings.TryGetValue(messageType, out var value) ? value : null;
		}

		public ushort GetPacketType(Type messageType)
		{
			return TypePacketTypeMappings.TryGetValue(messageType, out var type)
				? type
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
