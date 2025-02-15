using System;
using System.Collections.Generic;
using LiteNetLib.Utils;
using Scellecs.Morpeh;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers
{
    public sealed class ComponentSerializator
    {
        private readonly Dictionary<Type, object> _serializers = new Dictionary<Type, object>();
        private ComponentSerializatorFactory _componentSerializatorFactory;
        
        public ComponentSerializator(ComponentSerializatorFactory componentSerializatorFactory)
        {
            _componentSerializatorFactory = componentSerializatorFactory;
            
            Initialize();
        }

        private void Initialize()
        {
            RegisterAllSerializers();
        }

        private void RegisterAllSerializers()
        {
            var componentHandlers = _componentSerializatorFactory.GetAllComponentHandlers();

            foreach (var componentSerializer in componentHandlers)
            {
                RegisterSerializer(componentSerializer);
            }
            
            Debug.Log($"Component serializer were initialized! ({componentHandlers.Count})");
        }

        private void RegisterSerializer(object serializer)
        {
            var serializerType = serializer.GetType();
            var componentType = serializerType.GetInterfaces()[0].GetGenericArguments()[0];
            
            _serializers[componentType] = serializer;
        }
        
        public bool TrySerialize<T>(T component, out NetDataPackage dataPackage) where T : struct, IComponent
        {
            dataPackage = null;
            
            if (_serializers.TryGetValue(typeof(T), out var serializerObj))
            {
                var serializer = (IComponentSerializer<T>)serializerObj;
                serializer.Serialize(component, out dataPackage);
                return true;
            }
            
            Debug.LogWarning($"No serializer registered for component type: {typeof(T)}");
            return false;
        }

        public bool TryDeserialize<T>(NetDataPackage dataPackage, out T component) where T : struct, IComponent
        {
            component = default;
            if (_serializers.TryGetValue(typeof(T), out var serializerObj))
            {
                var serializer = (IComponentSerializer<T>)serializerObj;
                serializer.Deserialize(dataPackage, out component);
                return true;
            }
            
            Debug.LogWarning($"No serializer registered for component type: {typeof(T)}");
            return false;
        }
        
        public bool TryDeserializeToEntity<T>(NetDataPackage dataPackage, Entity entity) where T : struct, IComponent
        {
            if (TryDeserialize<T>(dataPackage, out var component))
            {
                entity.AddComponentData(component);
                return true;
            }
            return false;
        }
    }
}