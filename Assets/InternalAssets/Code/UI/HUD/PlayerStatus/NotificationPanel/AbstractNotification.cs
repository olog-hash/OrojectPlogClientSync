using System;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel
{
    public abstract class AbstractNotification
    {
        public event Action OnNotificationUpdated;
        public event Action OnNotificationClosed;

        public string NotificationMessage
        {
            get => _notificationMessage;
            protected set
            {
                _notificationMessage = value;
                OnNotificationUpdated?.Invoke();
            }
        }

        protected string _notificationMessage;

        public abstract void Initialize();
        public abstract void OnUpdate(float deltaTime);
        
        public virtual void Close()
        {
            OnNotificationClosed?.Invoke();
        }
    }
}