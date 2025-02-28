using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.PlayerStatus.HealthPanel;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus
{
    public class PlayerBlockViewModel : BaseViewModel
    {
        private NotificationViewModel _notificationViewModel;
        private HealthViewModel _healthViewModel;

        public PlayerBlockViewModel(NotificationViewModel notificationViewModel, HealthViewModel healthViewModel)
        {
            _notificationViewModel = notificationViewModel;
            _healthViewModel = healthViewModel;

            _notificationViewModel.OnNotificationCreated += OnNotificationCreated;
            _notificationViewModel.OnNotificationClosed += OnNotificationClosed;
            
        }

        private void OnNotificationCreated()
        {
            _healthViewModel.OnHide();
        }

        private void OnNotificationClosed()
        {
            _healthViewModel.OnShow();
        }
    }
}