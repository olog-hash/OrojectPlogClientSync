using ProjectOlog.Code.Features.Players.Instantiate;
using ProjectOlog.Code.Features.Players.Respawn;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Core;
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
    public sealed class UserDataInitDestroySystem : TickrateSystem
    {
        private Filter _initPlayerFilter;

        private NetworkUsersContainer _networkUsersContainer;

        public UserDataInitDestroySystem(NetworkUsersContainer networkUsersContainer)
        {
            _networkUsersContainer = networkUsersContainer;
        }

        public override void OnAwake()
        {
            _initPlayerFilter = World.Filter.With<PostInstantiatePlayerEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _initPlayerFilter)
            {
                ref var postInstantiatePlayerEvent = ref entityEvent.GetComponent<PostInstantiatePlayerEvent>();
                
                PostInitEvent(postInstantiatePlayerEvent);
            }
        }

        private void PostInitEvent(PostInstantiatePlayerEvent postInitEvent)
        {
            var playerEntity = postInitEvent.PlayerEntity;
            if (playerEntity is null || !playerEntity.Has<NetworkPlayer>()) return;
            
            ref var networkPlayer = ref playerEntity.GetComponent<NetworkPlayer>();
            if (!_networkUsersContainer.TryGetUserDataByID(networkPlayer.UserID, out var userData)) return;
            
            if (playerEntity.Has<DeadMarker>())
            {
                userData.GameState.SetDeathUser();
            }
            else
            {
                userData.GameState.ReviveUser();
            }
            
            // Обновляем информацию
            _networkUsersContainer.OnUsersUpdate?.Invoke();
        }
    }
}