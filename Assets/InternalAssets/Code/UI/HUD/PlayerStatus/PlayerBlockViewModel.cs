using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.PlayerStats.Presenter;
using ProjectOlog.Code.UI.HUD.PlayerStatus.HealthPanel;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Presenter;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus
{
    public class PlayerBlockViewModel : BaseViewModel
    {
        private NotificationViewModel _notificationViewModel;
        private PlayerStatsViewModel _playerStatsViewModel;

        public PlayerBlockViewModel(NotificationViewModel notificationViewModel, PlayerStatsViewModel playerStatsViewModel)
        {
            _notificationViewModel = notificationViewModel;
            _playerStatsViewModel = playerStatsViewModel;

            _notificationViewModel.IsLocalVisible.Subscribe(active =>
            {
                if (active)
                {
                    _playerStatsViewModel.Hide();
                }
                else
                {
                    _playerStatsViewModel.Show();
                }
            }).AddTo(_disposables);
        }
    }
}