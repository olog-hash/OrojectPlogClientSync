using ProjectOlog.Code.DataStorage.Core.Settings.Services;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.DataStorage.Core.Settings.Containers
{
    /// <summary>
    /// Контейнер игровых настроек
    /// </summary>
    public sealed class GameSettingsContainer : BaseSettingsContainer
    {
        // Реактивные свойства для настроек игры
        public ReactiveProperty<int> InterfaceMode { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<int> GameChat { get; } = new ReactiveProperty<int>(0);
        public ReactiveProperty<float> FieldOfView { get; } = new ReactiveProperty<float>(70);

        // Сервис для применения игровых настроек
        private GameSettingsService _service;

        public GameSettingsContainer()
        {
            // Создаем сервис и передаем себя в его конструктор
            _service = new GameSettingsService(this);
            LoadSettings();
        }

        public override void LoadSettings()
        {
            // Загрузка настроек из PlayerPrefs
            InterfaceMode.Value = PlayerPrefs.GetInt("Game_InterfaceMode", 0);
            GameChat.Value = PlayerPrefs.GetInt("Game_GameChat", 0);
            FieldOfView.Value = PlayerPrefs.GetFloat("Game_FieldOfView", 70f);
        }

        public override void SaveSettings()
        {
            // Сохранение настроек в PlayerPrefs
            PlayerPrefs.SetInt("Game_InterfaceMode", InterfaceMode.Value);
            PlayerPrefs.SetInt("Game_GameChat", GameChat.Value);
            PlayerPrefs.SetFloat("Game_FieldOfView", FieldOfView.Value);
            PlayerPrefs.Save();
        }

        public override void ApplySettings()
        {
            _service.ApplySettings();
        }

        public override void Dispose()
        {
            base.Dispose();
            
            // Очистка реактивных свойств
            InterfaceMode.Dispose();
            GameChat.Dispose();
            FieldOfView.Dispose();
        }
    }
}