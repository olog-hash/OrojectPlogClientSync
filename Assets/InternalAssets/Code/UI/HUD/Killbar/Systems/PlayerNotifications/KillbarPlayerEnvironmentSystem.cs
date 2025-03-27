using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Mechanics.Impact.Aggressors;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Network.Profiles.Users;
using ProjectOlog.Code.UI.HUD.Killbar;
using ProjectOlog.Code.UI.HUD.Killbar.Models;
using ProjectOlog.Code.UI.HUD.Killbar.Presenter;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using NetworkPlayer = ProjectOlog.Code.Network.Gameplay.Core.Components.NetworkPlayer;

namespace ProjectOlog.Code.UI.HUD.KillPanel.Systems.PlayerNotifications
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    //[UpdateAfter(typeof(PlayerDeathSystem))]
    public sealed class KillbarPlayerEnvironmentSystem : TickrateSystem
    {
        private Filter _playerDeathFilter;
        private KillbarViewModel _killbarViewModel;
        private NetworkUsersContainer _usersContainer;

        public KillbarPlayerEnvironmentSystem(KillbarViewModel killbarViewModel, NetworkUsersContainer usersContainer)
        {
            _killbarViewModel = killbarViewModel;
            _usersContainer = usersContainer;
        }
        
        public override void OnAwake()
        {
            _playerDeathFilter = World.Filter.With<DeathEvent>().With<EntityVictimEvent>().With<EnvironmentAggressorEvent>().Without<FakeEventMarker>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _playerDeathFilter)
            {
                ref var entityVictimEvent = ref entityEvent.GetComponent<EntityVictimEvent>();
                
                DeathEvent(entityVictimEvent, entityEvent);
            }
        }

        private void DeathEvent(EntityVictimEvent entityVictimEvent, Entity entityEvent)
        {
            if (!_usersContainer.TryGetUserDataByID(GetPlayerID(entityVictimEvent.VictimEntity), out var userData)) return;
            
            ref var environmentAggressorEvent = ref entityEvent.GetComponent<EnvironmentAggressorEvent>();
            
            var killbarItem = GetKillElement(userData, environmentAggressorEvent);
            _killbarViewModel.AddKillItem(killbarItem);
        }

        private KillbarItemModel GetKillElement(NetworkUserData userData, EnvironmentAggressorEvent environmentAggressorEvent)
        {
            string username = userData.Username;
            string environmentType = environmentAggressorEvent.EnvironmentType.ToString();
            bool isLocalPlayer = userData.ID == LocalData.LocalID;
            
            var builder = new KillbarBuilder();
            builder
                .AddFormattedText($"[{environmentType}]", Color.blue, marginRight: 4)
                .AddImage("gui/battle/icons/killfrag/bg_battle_headshot_my", width: 61, height: 15, marginRight: 4)
                .AddFormattedText(username, Color.white, fontSize: 12, marginRight: 4)
                .WithLifetime(5f);

            if (isLocalPlayer)
            {
                builder.AsLocalPlayer();
            }

            if (environmentAggressorEvent.EnvironmentType == EEnvironmentType.FallHight)
            {
                return KillbarBuilder.Suicide(username, "gui/battle/icons/killfrag/bg_battle_suicide_my", 5f,
                    isLocalPlayer);
            }

            return builder.Build();
        }

        private byte GetPlayerID(Entity entity)
        {
            if (entity.Has<NetworkPlayer>())
            {
                var networkPlayer = entity.GetComponent<NetworkPlayer>();

                return networkPlayer.UserID;
            }

            return 0;
        }
    }
}