using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Notifications;
using TMPro;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel
{
    public class DefaultNotificationView: AbstractScreen<NotificationViewModel>
    {
        [SerializeField] private TextMeshProUGUI _notificationText;
        
        private NotificationViewModel _currentViewModel;
        
        protected override void OnBind(NotificationViewModel model)
        {
            OnShowHideChanged(false);
            
            _currentViewModel = model;

            _currentViewModel.OnNotificationCreated += NotificationCreate;
            _currentViewModel.OnNotificationUpdated += NotificationUpdate;
            _currentViewModel.OnNotificationClosed += NotificationClose;
            
            // TODO remove
            _currentViewModel.ShowNotification<DefaultSpawnNotification>();
        }

        protected override void OnUnbind(NotificationViewModel model)
        {
            _currentViewModel.OnNotificationCreated -= NotificationCreate;
            _currentViewModel.OnNotificationUpdated -= NotificationUpdate;
            _currentViewModel.OnNotificationClosed -= NotificationClose;
        }

        private void NotificationCreate()
        {
            OnShowHideChanged(true);
            
            _notificationText.text = _currentViewModel.CurrentNotification?.NotificationMessage;
        }
        
        private void NotificationUpdate()
        {
            _notificationText.text = _currentViewModel.CurrentNotification?.NotificationMessage;
        }
        
        private void NotificationClose()
        {
            OnShowHideChanged(false);
        }

        private void OnShowHideChanged(bool isShown)
        {
            gameObject.SetActive(isShown);
        }
    }
}

// Подается запрос на создание такого-то уведомления.
// Он идет в фабрику где она инициализирует все уведомления.
// Далее инкапсулируется логика уведомления (с внедрением зависимостей), но он обновляется и управляется через интерфейс
// Уведомление (сам класс) инкапсулирует в себе логику предоставлению текст, а так же само решает когда удаляться.
// View получает данные какой текст ставить.