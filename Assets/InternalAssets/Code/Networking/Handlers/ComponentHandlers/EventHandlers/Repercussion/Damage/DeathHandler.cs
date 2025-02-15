using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Profiles.Entities;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers.EventHandlers.Repercussion.Damage
{
    public sealed class DeathHandler : IComponentSerializer<DeathEvent>
    {
        private NetworkEntitiesContainer _entitiesContainer;

        public DeathHandler(NetworkEntitiesContainer entitiesContainer)
        {
            _entitiesContainer = entitiesContainer;
        }
        
        public void Serialize(DeathEvent component, out NetDataPackage dataPackage)
        {
            ref var networkIdentity = ref component.VictimEntity.GetComponent<NetworkIdentity>();
            int serverID = networkIdentity.ServerID;

            dataPackage = new NetDataPackage(serverID);
        }

        public void Deserialize(NetDataPackage dataPackage, out DeathEvent component)
        {
            component = default;
            
            int serverID = dataPackage.GetInt();
            
            if (!_entitiesContainer.TryGetNetworkEntity(serverID, out var entityProvider)) return;

            component = new DeathEvent
            {
                VictimEntity = entityProvider.Entity,
            };
        }
    }
}