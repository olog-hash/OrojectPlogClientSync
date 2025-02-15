using ProjectOlog.Code._InDevs.Players.Visual.ShieldProtectPlayer.Events;
using ProjectOlog.Code.Networking.Profiles.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.Players.Visual.ShieldProtectPlayer
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ShieldPlayerProtectSystem : TickrateSystem
    {
        private Filter _addShieldFilter;
        private Filter _removeShieldFilter;
        private NetworkEntitiesContainer _networkEntitiesContainer;

        public ShieldPlayerProtectSystem(NetworkEntitiesContainer networkEntitiesContainer)
        {
            _networkEntitiesContainer = networkEntitiesContainer;
        }

        public override void OnAwake()
        {
            _addShieldFilter = World.Filter.With<ShieldAddedEvent>().Build();
            _removeShieldFilter = World.Filter.With<ShieldRemovedEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _addShieldFilter)
            {
                ref var addEvent = ref entity.GetComponent<ShieldAddedEvent>();
                ProcessShieldAdd(addEvent.ServerID);
            }
        
            foreach (var entity in _removeShieldFilter)
            {
                ref var removeEvent = ref entity.GetComponent<ShieldRemovedEvent>();
                ProcessShieldRemove(removeEvent.ServerID);
            }
        }

        private void ProcessShieldAdd(int serverID)
        {
            if (!_networkEntitiesContainer.TryGetNetworkEntity(serverID, out var entityProvider)) return;
            if (entityProvider is null || entityProvider.Entity is null) return;
            var playerEntity = entityProvider.Entity;
            
            ref var shield = ref playerEntity.GetComponent<ShieldProtectComponent>(out var exists);
            if (!exists) return;
        
            shield.IsActive = true;
            if (shield.ShieldObject != null)
            {
                shield.ShieldObject.LocalEnable = true;
            }
        }

        private void ProcessShieldRemove(int serverID)
        {
            if (!_networkEntitiesContainer.TryGetNetworkEntity(serverID, out var entityProvider)) return;
            if (entityProvider is null || entityProvider.Entity is null) return;
            var playerEntity = entityProvider.Entity;
            
            ref var shield = ref playerEntity.GetComponent<ShieldProtectComponent>(out var exists);
            if (!exists) return;

            shield.IsActive = false;
            if (shield.ShieldObject != null)
            {
                shield.ShieldObject.LocalEnable = false;
            }
        }
    }
}