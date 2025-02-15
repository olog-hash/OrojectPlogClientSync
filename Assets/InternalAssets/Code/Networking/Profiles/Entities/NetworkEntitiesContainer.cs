using ProjectOlog.Code.Gameplay.ECS.Rules;
using ProjectOlog.Code.Networking.Profiles.Entities.Containers;
using Scellecs.Morpeh.Providers;

namespace ProjectOlog.Code.Networking.Profiles.Entities
{
    public class NetworkEntitiesContainer
    {
        public EcsRules EcsRules;

        public PlayerEntityContainer PlayerEntities { get; }
        public ObjectEntityContainer ObjectEntities { get; }

        public NetworkEntitiesContainer()
        {
            PlayerEntities = new PlayerEntityContainer();
            ObjectEntities = new ObjectEntityContainer();

            ClearContainer();
        }

        public void ClearContainer()
        {
            EcsRules = null;
            
            ObjectEntities.Clear();
            PlayerEntities.Clear();
        }

        public void RegisterEcsRules(EcsRules ecsRules)
        {
            EcsRules = ecsRules;
        }

        public EntityProvider GetNetworkEntity(int id)
        {
            return PlayerEntities.GetNetworkEntity(id) ?? 
                   ObjectEntities.GetNetworkEntity(id);
        }

        public bool TryGetNetworkEntity(int id, out EntityProvider entityProvider)
        {
            entityProvider = GetNetworkEntity(id);
            return entityProvider != null;
        }
        
        public bool RemoveNetworkEntity(int id)
        {
            return PlayerEntities.RemoveNetworkEntity(id) || 
                   ObjectEntities.RemoveNetworkEntity(id);
        }
    }
}