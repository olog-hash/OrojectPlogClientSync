using ProjectOlog.Code.Features.Players.Visual.PanelInfo;
using ProjectOlog.Code.Network.Gameplay.Core.Components;
using ProjectOlog.Code.Network.Infrastructure.SubComponents.Core;
using ProjectOlog.Code.Network.Profiles.Users;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Features.Players.Instantiate
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class InstantiatePlayerPresentationSystem : TickrateSystem
    {
        private Filter _initPlayerFilter;

        private NetworkUsersContainer _networkUsersContainer;

        public InstantiatePlayerPresentationSystem(NetworkUsersContainer networkUsersContainer)
        {
            _networkUsersContainer = networkUsersContainer;
        }

        public override void OnAwake()
        {
            _initPlayerFilter = World.Filter.With<InstantiatePlayerEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _initPlayerFilter)
            {
                ref var instantiatePlayerEvent = ref entityEvent.GetComponent<InstantiatePlayerEvent>();
                ref var mapping = ref instantiatePlayerEvent.EntityProviderMappingPool;
                var packet = instantiatePlayerEvent.InstantiatePlayerPacket;

                ProcessPlayersName(ref mapping);
            }
        }
        
        // Обрабатываем имена пользователей (таблички над головой)
        private void ProcessPlayersName(ref EntityProviderMappingPool mapping)
        {
            foreach (var provider in mapping.EventIDToEntityProvider.Values)
            {
                if (!provider.Has<NetworkPlayer>()) continue;
                
                if (provider.TryGetComponent<PlayerInfoPanel>(out var playerInfoPanel))
                {
                    var networkPlayer = provider.Entity.GetComponent<NetworkPlayer>();

                    if (_networkUsersContainer.TryGetUserDataByID(networkPlayer.UserID, out var userData))
                    {
                        playerInfoPanel.Initialize(userData.Username);
                    }
                }
            }
        }
    }
}