using ProjectOlog.Code._InDevs.Players.Respawn;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.Networking.Game.Core;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.Visual.Ragdoll
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RespawnRagdollSystem : TickrateSystem
    {
        private Filter _spawnPlayerFilter;
        private Filter _playerDeathFilter;
        
        public override void OnAwake()
        {
            _spawnPlayerFilter = World.Filter.With<RespawnPlayerEvent>().Build();
            _playerDeathFilter = World.Filter.With<DeathEvent>().With<EntityVictimEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _playerDeathFilter)
            {
                ref var entityVictimEvent = ref entityEvent.GetComponent<EntityVictimEvent>();
                
                DeathEvent(entityVictimEvent, entityEvent);
            }
            
            // Мониторим ивенты спавна
            foreach (var entityEvent in _spawnPlayerFilter)
            {
                ref var spawnEvent = ref entityEvent.GetComponent<RespawnPlayerEvent>();

                SpawnPlayer(spawnEvent);
            }
        }

        private void DeathEvent(EntityVictimEvent entityVictimEvent, Entity entityEvent)
        {
            var playerEntity = entityVictimEvent.VictimEntity;
            if (playerEntity is null || !playerEntity.Has<NetworkPlayer>() || !playerEntity.Has<PlayerRagdollComponent>()) return;

            EnableCollider(playerEntity, true);
            
            ref var playerRagdollComponent = ref playerEntity.GetComponent<PlayerRagdollComponent>();

            playerRagdollComponent.Animator.enabled = false;
            playerRagdollComponent.RagdollOperations.EnableRagdoll();
        }
        
        public void SpawnPlayer(RespawnPlayerEvent respawnEvent)
        {
            var playerProvider = respawnEvent.PlayerProvider;
            if (playerProvider is null || !playerProvider.Has<PlayerRagdollComponent>()) return;

            EnableCollider(playerProvider.Entity, false);
            
            ref var playerRagdollComponent = ref playerProvider.Entity.GetComponent<PlayerRagdollComponent>();

            // Сначала включаем аниматор
            playerRagdollComponent.Animator.enabled = true;
        
            // Сбрасываем состояние аниматора
            playerRagdollComponent.Animator.Rebind();
            playerRagdollComponent.Animator.Update(0f);
        
            // Отключаем рэгдолл после сброса анимации
            playerRagdollComponent.RagdollOperations.DisableRagdoll();
        }
        
        private void EnableCollider(Entity playerEntity, bool flag)
        {
            ref var translation = ref playerEntity.GetComponent<Translation>(out var exist);
            if (!exist) return;

            if (translation.Transform.TryGetComponent(out CapsuleCollider capsuleCollider))
            {
                capsuleCollider.isTrigger = flag;
            }
        }
    }
}