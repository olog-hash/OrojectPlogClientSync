using System;
using R3;

namespace ProjectOlog.Code.UI.Shared.Settings.Presenter
{
    // Базовая модель для всех вкладок настроек
    public abstract class BaseSettingsModel : IDisposable
    {
        // Флаг для отслеживания изменений
        private ReactiveProperty<bool> _hasChanges = new ReactiveProperty<bool>(false);
        public ReadOnlyReactiveProperty<bool> HasChanges => _hasChanges.ToReadOnlyReactiveProperty();
    
        protected CompositeDisposable _disposables = new CompositeDisposable();
    
        // Отметка об изменениях
        protected void SetHasChanges(bool value)
        {
            _hasChanges.Value = value;
        }
    
        public abstract void ApplySettings();
        public abstract void ResetToDefault();
    
        public virtual void Dispose()
        {
            _hasChanges.Dispose();
            _disposables.Dispose();
        }
    }
}