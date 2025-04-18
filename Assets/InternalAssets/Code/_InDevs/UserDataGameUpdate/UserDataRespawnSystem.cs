﻿using ProjectOlog.Code._InDevs.Players.Respawn;
using ProjectOlog.Code.Mechanics.Repercussion.Core.Victims;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.Networking.Game.Core;
using ProjectOlog.Code.Networking.Profiles.Users;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.UserDataGameUpdate
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class UserDataRespawnSystem : TickrateSystem
    {
        private Filter _spawnPlayerFilter;
        private Filter _playerDeathFilter;

        private NetworkUsersContainer _networkUsersContainer;

        public UserDataRespawnSystem(NetworkUsersContainer networkUsersContainer)
        {
            _networkUsersContainer = networkUsersContainer;
        }

        public override void OnAwake()
        {
            _spawnPlayerFilter = World.Filter.With<RespawnPlayerEvent>().Build();
            _playerDeathFilter = World.Filter.With<DeathEvent>().With<PlayerVictimMarker>().Without<VirtualEventMarker>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _playerDeathFilter)
            {
                ref var deathEvent = ref entityEvent.GetComponent<DeathEvent>();
                
                DeathEvent(deathEvent, entityEvent);
            }
            
            // Мониторим ивенты спавна
            foreach (var entityEvent in _spawnPlayerFilter)
            {
                ref var spawnEvent = ref entityEvent.GetComponent<RespawnPlayerEvent>();

                SpawnPlayer(spawnEvent);
            }
        }
        
        private void DeathEvent(DeathEvent deathEvent, Entity entityEvent)
        {
            var playerEntity = deathEvent.VictimEntity;
            if (playerEntity is null || !playerEntity.Has<NetworkPlayer>()) return;

            ref var networkPlayer = ref playerEntity.GetComponent<NetworkPlayer>();
            if (!_networkUsersContainer.TryGetUserDataByID(networkPlayer.UserID, out var userData)) return;
            
            // Обновляем информацию в userData
            userData.IsDead = true;
            userData.DeathCount++;
            
            // Обновляем информацию
            _networkUsersContainer.OnUsersUpdate?.Invoke();
        }
        
        public void SpawnPlayer(RespawnPlayerEvent respawnEvent)
        {
            var playerEntity = respawnEvent.PlayerProvider.Entity;
            if (playerEntity is null || !playerEntity.Has<NetworkPlayer>()) return;

            ref var networkPlayer = ref playerEntity.GetComponent<NetworkPlayer>();
            if (!_networkUsersContainer.TryGetUserDataByID(networkPlayer.UserID, out var userData)) return;
            
            // Обновляем информацию в userData
            userData.IsDead = false;
            
            // Обновляем информацию
            _networkUsersContainer.OnUsersUpdate?.Invoke();
        }
    }
}