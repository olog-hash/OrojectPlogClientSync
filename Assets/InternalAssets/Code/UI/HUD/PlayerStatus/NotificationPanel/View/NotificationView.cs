using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Notifications;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Presenter;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.View
{
    public class NotificationView : UIToolkitScreen<NotificationViewModel>
    {
        private Label _notificationText;

        protected override void SetVisualElements()
        {
            _notificationText = _root.Q<Label>("notification-text");
                
            if (_notificationText == null)
            {
                Debug.LogWarning($"notification-message Label not found in UXML for {name}");
            }
        }
        
        protected override void OnBind(NotificationViewModel model)
        {
            // Подписка на изменение текущего уведомления
            model.CurrentNotification
                .Subscribe(notification => {
                    if (notification != null)
                    {
                        // Обновляем текст уведомления
                        notification.NotificationMessage
                            .Subscribe(message => {
                                if (_notificationText != null)
                                    _notificationText.text = message;
                            })
                            .AddTo(_disposables);
                        
                        // Показываем экран
                        Show();
                    }
                    else
                    {
                        // Если уведомление null, скрываем экран
                        Hide();
                    }
                })
                .AddTo(_disposables);
            
            // Для тестирования
            model.ShowNotification<DefaultSpawnNotification>();
        }

        protected override void OnUnbind(NotificationViewModel model)
        {
            // Очистка подписок происходит автоматически через _disposables
        }
    }
}