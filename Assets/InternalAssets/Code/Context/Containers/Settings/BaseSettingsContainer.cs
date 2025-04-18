using R3;

namespace ProjectOlog.Code.DataStorage.Core.Settings
{
    /// <summary>
    /// Базовый контейнер настроек, предоставляющий общую функциональность
    /// </summary>
    public abstract class BaseSettingsContainer : IProjectContainer
    {
        protected CompositeDisposable _disposables = new CompositeDisposable();
        
        public BaseSettingsContainer()
        {
            LoadSettings();
        }
        
        // Загрузка настроек из постоянного хранилища
        public abstract void LoadSettings();
        
        // Сохранение настроек в постоянное хранилище
        public abstract void SaveSettings();
        public abstract void ApplySettings();
        
        // Реализация интерфейса IProjectContainer
        public void Reset()
        {
            // При сбросе контейнера загружаем сохраненные настройки
            LoadSettings();
        }
        
        public virtual void Dispose()
        {
            _disposables.Dispose();
        }
    }
}