using LiteNetLib.Utils;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers.AdvancedDeserializers.Core
{
    public interface IComponentTypeHandler
    {
        bool TryDeserialize(ComponentSerializator serializator, NetDataPackage dataPackage, Entity entity);
    }
}