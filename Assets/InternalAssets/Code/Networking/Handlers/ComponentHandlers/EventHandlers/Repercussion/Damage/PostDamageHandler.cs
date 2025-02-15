using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.DamageRequest;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Profiles.Entities;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Handlers.ComponentHandlers.EventHandlers.Repercussion.Damage
{
    public sealed class PostDamageHandler : IComponentSerializer<PostDamageEvent>
    {
        private NetworkEntitiesContainer _entitiesContainer;

        public PostDamageHandler(NetworkEntitiesContainer entitiesContainer)
        {
            _entitiesContainer = entitiesContainer;
        }

        public void Serialize(PostDamageEvent component, out NetDataPackage dataPackage)
        {
            ref var networkIdentity = ref component.VictimEntity.GetComponent<NetworkIdentity>();

            int serverID = networkIdentity.ServerID;
            int realDamageCount = component.RealDamageCount;

            dataPackage = new NetDataPackage(serverID, realDamageCount);
        }

        public void Deserialize(NetDataPackage dataPackage, out PostDamageEvent component)
        {
            component = default;
            
            int serverID = dataPackage.GetInt();
            int realDamageCount = dataPackage.GetInt();

            if (!_entitiesContainer.TryGetNetworkEntity(serverID, out var entityProvider)) return;
            
            component = new PostDamageEvent
            {
                VictimEntity = entityProvider.Entity,
                RealDamageCount = realDamageCount,
            };
        }
    }
}