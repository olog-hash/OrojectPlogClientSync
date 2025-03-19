using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Notifications;
using Zenject;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel
{
    public class NotificationFactory
    {
        private readonly DiContainer _container;
        
        public NotificationFactory(DiContainer container)
        {
            _container = container;
        }

        public T CreateNotification<T>() where T : AbstractNotification
        {
            return _container.Resolve<T>();
        }
    }
}