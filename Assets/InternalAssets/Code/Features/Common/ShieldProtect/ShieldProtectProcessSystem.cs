using ProjectOlog.Code.Features.Entities.ShieldProtect.Events;
using ProjectOlog.Code.Network.Profiles.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Entities.ShieldProtect
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ShieldPlayerProtectSystem : TickrateSystem
    {
        private Filter _addShieldFilter;
        private Filter _removeShieldFilter;
        private Filter _shieldFilter;
        
        private NetworkEntitiesContainer _networkEntitiesContainer;

        public ShieldPlayerProtectSystem(NetworkEntitiesContainer networkEntitiesContainer)
        {
            _networkEntitiesContainer = networkEntitiesContainer;
        }

        public override void OnAwake()
        {
            _addShieldFilter = World.Filter.With<ShieldAddedEvent>().Build();
            _removeShieldFilter = World.Filter.With<ShieldRemovedEvent>().Build();
            _shieldFilter = World.Filter.With<ShieldProtectComponent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entity in _addShieldFilter)
            {
                ref var addEvent = ref entity.GetComponent<ShieldAddedEvent>();
                ProcessShieldAdd(addEvent.ServerID, addEvent.ShieldTime);
            }
    
            foreach (var entity in _removeShieldFilter)
            {
                ref var removeEvent = ref entity.GetComponent<ShieldRemovedEvent>();
                ProcessShieldRemove(removeEvent.ServerID);
            }

            // Обновляем время на щите
            foreach (var entity in _shieldFilter)
            {
                ref var shield = ref entity.GetComponent<ShieldProtectComponent>();
                shield.ShieldTime -= deltaTime;

                if (shield.ShieldTime <= 0)
                {
                    UpdateShieldState(entity, false);
                }
            }
        }

        private void ProcessShieldAdd(ushort serverID, float shieldTime)
        {
            if (!_networkEntitiesContainer.TryGetNetworkEntity(serverID, out var entityProvider)) return;
            if (entityProvider is null || entityProvider.Entity is null) return;

            UpdateShieldState(entityProvider.Entity, true, shieldTime);
        }

        private void ProcessShieldRemove(ushort serverID)
        {
            if (!_networkEntitiesContainer.TryGetNetworkEntity(serverID, out var entityProvider)) return;
            if (entityProvider is null || entityProvider.Entity is null) return;

            UpdateShieldState(entityProvider.Entity, false);
        }

        private void UpdateShieldState(Entity entity, bool flag, float shieldTime = 0)
        {
            if (!entity.Has<ShieldProtectComponent>()) return;
            
            ref var shieldProtectComponent = ref entity.GetComponent<ShieldProtectComponent>();

            shieldProtectComponent.IsActive = flag;
            shieldProtectComponent.ShieldTime = shieldTime;
            if (shieldProtectComponent.ShieldObject != null)
            {
                shieldProtectComponent.ShieldObject.LocalEnable = flag;
            }
        }
    }
}