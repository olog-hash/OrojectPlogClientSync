using LiteNetLib.Utils;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers
{
    public interface IComponentSerializer<T> where T : struct, IComponent
    {
        void Serialize(T component, out NetDataPackage dataPackage);
        void Deserialize(NetDataPackage dataPackage, out T component);
    }
}