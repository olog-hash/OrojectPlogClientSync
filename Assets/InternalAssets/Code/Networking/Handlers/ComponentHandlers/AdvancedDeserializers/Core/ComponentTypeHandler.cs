using LiteNetLib.Utils;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers.AdvancedDeserializers.Core
{
    public class ComponentTypeHandler<T> : IComponentTypeHandler where T : struct, IComponent
    {
        public bool TryDeserialize(ComponentSerializator serializator, NetDataPackage dataPackage, Entity entity)
        {
            if (serializator.TryDeserialize<T>(dataPackage, out var component))
            {
                entity.AddComponentData(component);
                return true;
            }
            return false;
        }
    }
}