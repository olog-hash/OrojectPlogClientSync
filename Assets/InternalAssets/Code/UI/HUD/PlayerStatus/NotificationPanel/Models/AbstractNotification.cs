using System;
using R3;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Notifications
{
    public abstract class AbstractNotification : IDisposable
    {
        public ReactiveProperty<string> NotificationMessage { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> IsActive { get; } = new ReactiveProperty<bool>(true);
        
        protected CompositeDisposable _disposables = new CompositeDisposable();

        public abstract void Initialize();
        
        // Метод будет вызываться из системы обновлений (например, через UniTask)
        public abstract void OnUpdate(float deltaTime);
        
        public virtual void Close()
        {
            IsActive.Value = false;
        }
        
        public virtual void Dispose()
        {
            IsActive.Dispose();
            NotificationMessage.Dispose();
            
            _disposables.Dispose();
        }
    }
}