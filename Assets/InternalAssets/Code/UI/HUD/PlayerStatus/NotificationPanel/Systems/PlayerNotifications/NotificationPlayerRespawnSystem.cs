using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Mechanics.Repercussion.Core.Victims;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Notifications;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Systems.PlayerNotifications
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NotificationPlayerRespawnSystem : TickrateSystem
    {
        private Filter _playerDeathFilter;
        private NotificationViewModel _notificationViewModel;

        public NotificationPlayerRespawnSystem(NotificationViewModel notificationViewModel)
        {
            _notificationViewModel = notificationViewModel;
        }
        
        public override void OnAwake()
        {
            _playerDeathFilter = World.Filter.With<DeathEvent>().With<PlayerVictimMarker>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _playerDeathFilter)
            {
                ref var deathEvent = ref entityEvent.GetComponent<DeathEvent>();
                
                DeathEvent(deathEvent, entityEvent);
            }
        }

        private void DeathEvent(DeathEvent deathEvent, Entity entityEvent)
        {
            if (!deathEvent.VictimEntity.Has<LocalPlayerMarker>()) return;
            
            _notificationViewModel.ShowNotification<DefaultSpawnNotification>();
        }
    }
}