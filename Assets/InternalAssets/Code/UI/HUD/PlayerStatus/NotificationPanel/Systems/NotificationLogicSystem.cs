using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class NotificationLogicSystem : TickrateSystem
    {
        private NotificationViewModel _notificationViewModel;

        public NotificationLogicSystem(NotificationViewModel notificationViewModel)
        {
            _notificationViewModel = notificationViewModel;
        }

        public override void OnAwake()
        {

        }

        public override void OnUpdate(float deltaTime)
        {
            _notificationViewModel.OnUpdate(deltaTime);
        }
    }
}