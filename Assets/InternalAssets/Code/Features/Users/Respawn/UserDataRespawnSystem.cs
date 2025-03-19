using ProjectOlog.Code.Features.Players.Respawn;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Network.Gameplay.Core.Components;
using ProjectOlog.Code.Network.Profiles.Users;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Network.Gameplay.UserDataGameUpdate
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
            _playerDeathFilter = World.Filter.With<DeathEvent>().With<EntityVictimEvent>().Without<VirtualEventMarker>().Build();
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
            if (playerEntity is null || !playerEntity.Has<NetworkPlayer>()) return;

            ref var networkPlayer = ref playerEntity.GetComponent<NetworkPlayer>();
            if (!_networkUsersContainer.TryGetUserDataByID(networkPlayer.UserID, out var userData)) return;
            
            // Обновляем информацию в userData
            userData.GameState.DeathUser();
            
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
            userData.GameState.ReviveUser();
            
            // Обновляем информацию
            _networkUsersContainer.OnUsersUpdate?.Invoke();
        }
    }
}