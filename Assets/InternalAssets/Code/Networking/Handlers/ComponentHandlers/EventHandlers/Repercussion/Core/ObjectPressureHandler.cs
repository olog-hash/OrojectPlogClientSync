using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Repercussion.Core.Pressures;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Profiles.Entities;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers.EventHandlers.Repercussion.Core
{
    public sealed class ObjectPressureHandler : IComponentSerializer<ObjectPressure>
    {
        private NetworkEntitiesContainer _entitiesContainer;

        public ObjectPressureHandler(NetworkEntitiesContainer entitiesContainer)
        {
            _entitiesContainer = entitiesContainer;
        }
        
        public void Serialize(ObjectPressure component, out NetDataPackage dataPackage)
        {
            ref var networkIdentity = ref component.OwnerEntity.GetComponent<NetworkIdentity>();
            int serverID = networkIdentity.ServerID;

            dataPackage = new NetDataPackage(serverID);
        }

        public void Deserialize(NetDataPackage dataPackage, out ObjectPressure component)
        {
            component = default;
            
            int serverID = dataPackage.GetInt();
            
            if (!_entitiesContainer.TryGetNetworkEntity(serverID, out var entityProvider)) return;

            component = new ObjectPressure()
            {
                OwnerEntity = entityProvider.Entity,
            };
        }
    }
}