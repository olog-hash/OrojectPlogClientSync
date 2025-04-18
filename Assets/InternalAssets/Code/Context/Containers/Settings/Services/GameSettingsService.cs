using ProjectOlog.Code.DataStorage.Core.Settings.Containers;
using UnityEngine;

namespace ProjectOlog.Code.DataStorage.Core.Settings.Services
{
    /// <summary>
    /// Сервис для применения игровых настроек
    /// </summary>
    public sealed class GameSettingsService
    {
        private readonly GameSettingsContainer _container;

        public GameSettingsService(GameSettingsContainer container)
        {
            _container = container;
        }

        public void ApplySettings()
        {
            ApplyInterfaceSettings();
            ApplyFieldOfView();
            
            _container.SaveSettings();
        }

        private void ApplyInterfaceSettings()
        {
            // Логика применения настроек интерфейса
            // Зависит от конкретной реализации UI менеджера в игре}

        }

        private void ApplyFieldOfView()
        {
            // Логика применения поля зрения
            // Например: Camera.main.fieldOfView = _container.FieldOfView.Value;
        }
    }
}