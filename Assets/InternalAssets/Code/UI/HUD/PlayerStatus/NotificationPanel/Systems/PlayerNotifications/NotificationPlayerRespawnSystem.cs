using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Death;
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
            _playerDeathFilter = World.Filter.With<DeathEvent>().With<EntityVictimEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            foreach (var entityEvent in _playerDeathFilter)
            {
                ref var entityVictimEvent = ref entityEvent.GetComponent<EntityVictimEvent>();

                if (entityVictimEvent.IsLocalPlayer() && entityVictimEvent.IsNetworkPlayer())
                {
                    _notificationViewModel.ShowNotification<DefaultSpawnNotification>();
                }
            }
        }
    }
}