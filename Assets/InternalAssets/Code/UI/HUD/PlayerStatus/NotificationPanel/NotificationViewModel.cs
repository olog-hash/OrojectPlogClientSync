using System;
using ProjectOlog.Code.UI.Core;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel
{
    public class NotificationViewModel : BaseViewModel
    {
        public event Action OnNotificationCreated;
        public event Action OnNotificationUpdated;
        public event Action OnNotificationClosed;

        public AbstractNotification CurrentNotification { get; private set; }
        
        private NotificationFactory _notificationFactory;

        public NotificationViewModel(NotificationFactory notificationFactory)
        {
            _notificationFactory = notificationFactory;
        }

        public void ShowNotification<T>() where T : AbstractNotification
        {
            CloseCurrentNotification();
            CurrentNotification = _notificationFactory.CreateNotification<T>();
            
            CurrentNotification.Initialize();
            CurrentNotification.OnNotificationUpdated += OnNotificationUpdated;
            CurrentNotification.OnNotificationClosed += HandleNotificationClosed;
            
            OnNotificationCreated?.Invoke();
        }
        
        public void CloseCurrentNotification()
        {
            CurrentNotification?.Close();
        }

        public void OnUpdate(float deltaTime)
        {
            CurrentNotification?.OnUpdate(deltaTime);
        }
        
        private void HandleNotificationClosed()
        {
            CurrentNotification.OnNotificationUpdated -= OnNotificationUpdated;
            CurrentNotification.OnNotificationClosed -= HandleNotificationClosed;
            CurrentNotification = null;
            
            OnNotificationClosed?.Invoke();
        }
    }
}