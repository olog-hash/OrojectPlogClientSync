using System;
using System.Collections.Generic;
using LiteNetLib.Utils;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers.AdvancedDeserializers.Core
{
    public class GenericComponentDeserializer
    {
        private readonly ComponentSerializator _componentSerializator;
        private readonly Dictionary<int, IComponentTypeHandler> _handlers;

        public GenericComponentDeserializer(ComponentSerializator componentSerializator)
        {
            _componentSerializator = componentSerializator;
            _handlers = new Dictionary<int, IComponentTypeHandler>();
        }

        public void RegisterHandler<TEnum, TComponent>(TEnum enumValue)
            where TEnum : Enum
            where TComponent : struct, IComponent
        {
            int key = Convert.ToInt32(enumValue);
            _handlers[key] = new ComponentTypeHandler<TComponent>();
        }

        public bool TryDeserializeToEntity<TEnum>(TEnum enumValue, NetDataPackage dataPackage, Entity entity)
            where TEnum : Enum
        {
            int key = Convert.ToInt32(enumValue);
            if (_handlers.TryGetValue(key, out var handler))
            {
                return handler.TryDeserialize(_componentSerializator, dataPackage, entity);
            }

            return false;
        }
    }
}