using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Notifications;
using R3;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Presenter
{
    public class NotificationViewModel : BaseViewModel
    {
        public ReactiveProperty<AbstractNotification> CurrentNotification { get; } = new ReactiveProperty<AbstractNotification>();
        
        private NotificationFactory _notificationFactory;
        
        public NotificationViewModel(NotificationFactory notificationFactory)
        {
            _notificationFactory = notificationFactory;
            
            // Автоматически закрываем экран, когда уведомление становится неактивным
            CurrentNotification
                .Where(n => n != null)
                .SelectMany(n => n.IsActive)
                .Where(isActive => !isActive)
                .Subscribe(_ => HandleNotificationClosed())
                .AddTo(_disposables);
        }

        public void OnUpdate(float deltaTime)
        {
            CurrentNotification.Value?.OnUpdate(deltaTime);
        }

        public void ShowNotification<T>() where T : AbstractNotification
        {
            // Закрываем старое уведомление
            CurrentNotification.Value?.Dispose();

            // Создаем новое уведомление
            var notification = _notificationFactory.CreateNotification<T>();
            notification.Initialize();

            // Устанавливаем новое уведомление
            CurrentNotification.Value = notification;
            
            Show();
        }

        public void CloseCurrentNotification()
        {
            CurrentNotification.Value?.Close();
        }

        private void HandleNotificationClosed()
        {
            var notification = CurrentNotification.Value;
            if (notification != null)
            {
                notification.Dispose();
                CurrentNotification.Value = null;
                
                Hide();
            }
        }

        public override void Dispose()
        {
            CurrentNotification.Value?.Dispose();
            CurrentNotification.Dispose();
            base.Dispose();
        }
    }
}