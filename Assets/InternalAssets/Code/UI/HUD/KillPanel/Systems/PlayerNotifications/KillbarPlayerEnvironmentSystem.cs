using ProjectOlog.Code.Mechanics.Impact.Aggressors;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.Networking.Profiles.Users;
using ProjectOlog.Code.UI.HUD.KillPanel.Builder;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using NetworkPlayer = ProjectOlog.Code.Networking.Game.Core.NetworkPlayer;

namespace ProjectOlog.Code.UI.HUD.KillPanel.Systems.PlayerNotifications
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    //[UpdateAfter(typeof(PlayerDeathSystem))]
    public sealed class KillbarPlayerEnvironmentSystem : TickrateSystem
    {
        private Filter _playerDeathFilter;
        private KillBarViewModel _killbarViewModel;
        private NetworkUsersContainer _usersContainer;

        public KillbarPlayerEnvironmentSystem(KillBarViewModel killbarViewModel, NetworkUsersContainer usersContainer)
        {
            _killbarViewModel = killbarViewModel;
            _usersContainer = usersContainer;
        }
        
        public override void OnAwake()
        {
            _playerDeathFilter = World.Filter.With<DeathEvent>().With<EntityVictimEvent>().With<EnvironmentAggressorEvent>().Without<VirtualEventMarker>().Build();
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

            string userName = userData.Username;
            string environmentType = environmentAggressorEvent.EnvironmentType.ToString();
            
            var builder1 = new KillMessageBuilder();
            builder1
                .AddTextElement($"{userName}", Color.blue)
                .AddTextElement("был убит", Color.red)
                .AddTextElement($"[{environmentType}]", Color.blue);
                
            var messageData1 = builder1.Build(5f);
                
            _killbarViewModel.AddKillMessage(messageData1);
        }

        private int GetPlayerID(Entity entity)
        {
            if (entity.Has<NetworkPlayer>())
            {
                var networkPlayer = entity.GetComponent<NetworkPlayer>();

                return networkPlayer.UserID;
            }

            return -1;
        }
    }
}