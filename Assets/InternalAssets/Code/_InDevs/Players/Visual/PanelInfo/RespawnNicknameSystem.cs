﻿using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code._InDevs.Players.Respawn;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Mechanics.Repercussion.Core.Victims;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.Players.Visual.PanelInfo
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class RespawnNicknameSystem : TickrateSystem
    {
        private Filter _spawnPlayerFilter;
        private Filter _playerDeathFilter;
        
        public override void OnAwake()
        {
            _spawnPlayerFilter = World.Filter.With<RespawnPlayerEvent>().Build();
            _playerDeathFilter = World.Filter.With<DeathEvent>().With<PlayerVictimMarker>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            // Мониторим ивенты спавна
            foreach (var entityEvent in _spawnPlayerFilter)
            {
                ref var spawnEvent = ref entityEvent.GetComponent<RespawnPlayerEvent>();

                SpawnEvent(spawnEvent);
            }
            
            // Мониторим ивенты смерти
            foreach (var entityEvent in _playerDeathFilter)
            {
                ref var deathEvent = ref entityEvent.GetComponent<DeathEvent>();
                
                DeathEvent(deathEvent, entityEvent);
            }
        }

        public void SpawnEvent(RespawnPlayerEvent respawnEvent)
        {
            var playerProvider = respawnEvent.PlayerProvider;
            if (playerProvider is null || playerProvider.Entity.Has<LocalPlayerMarker>()) return;

            EnableNicknamePanel(playerProvider.Entity, true);
        }
        
        private void DeathEvent(DeathEvent deathEvent, Entity entityEvent)
        {
            var playerEntity = deathEvent.VictimEntity;
            if (playerEntity is null || playerEntity.Has<LocalPlayerMarker>()) return;

            EnableNicknamePanel(playerEntity, false);
        }
        
        private void EnableNicknamePanel(Entity playerEntity, bool flag)
        {
            ref var translation = ref playerEntity.GetComponent<Translation>(out var exist);
            if (!exist) return;

            if (translation.Transform.TryGetComponent<PlayerInfoPanel>(out var playerInfoPanel))
            {
                playerInfoPanel.SetActivePanel(flag);
            }
        }
    }
}