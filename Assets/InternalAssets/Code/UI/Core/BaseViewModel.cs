using System;

namespace ProjectOlog.Code.UI.Core
{
    public interface IViewModel : IDisposable
    {
        
    }

    public abstract class BaseViewModel : IViewModel
    {
        public bool IsActiveView => _isActiveView;
        protected bool _isActiveView = true;

        public virtual void Show()
        {
            _isActiveView = true;
        }

        public virtual void Hide()
        {
            _isActiveView = false;
        }
        
        public virtual void Dispose() { }
    }
}