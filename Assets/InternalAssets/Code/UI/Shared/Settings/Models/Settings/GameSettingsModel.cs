using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.Shared.Settings.Presenter
{
    public class GameSettingsModel : BaseSettingsModel
    {
        // Реактивные свойства для настроек игры
        public ReactiveProperty<int> InterfaceMode { get; } = new ReactiveProperty<int>(0); // 0 - Полностью, 1 - Только оружие, 2 - Отключен
        public ReactiveProperty<int> GameChat { get; } = new ReactiveProperty<int>(0); // 0 - вкл, 1 - выкл
        public ReactiveProperty<float> FieldOfView { get; } = new ReactiveProperty<float>(70); // от 60 до 120

        // Опции для элементов UI
        public string[] InterfaceOptions { get; } = { "Полностью", "Только оружие", "Отключен" };
        public string[] ChatOptions { get; } = { "Вкл", "Выкл" };

        // Значения по умолчанию
        private readonly int _defaultInterfaceMode = 0; // По умолчанию "Полностью"
        private readonly int _defaultGameChat = 0; // По умолчанию "Вкл"
        private readonly float _defaultFieldOfView = 70f; // По умолчанию среднее значение

        public GameSettingsModel()
        {
            // Инициализация
            SetupChangeTracking();
            LoadSettings();
        }

        private void SetupChangeTracking()
        {
            // Подписка на все изменения чтобы отслеживать изменения настроек
            InterfaceMode.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            GameChat.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
            FieldOfView.Subscribe(_ => SetHasChanges(true)).AddTo(_disposables);
        }

        private void LoadSettings()
        {
            // Загрузка настроек из PlayerPrefs
            InterfaceMode.Value = PlayerPrefs.GetInt("Game_InterfaceMode", _defaultInterfaceMode);
            GameChat.Value = PlayerPrefs.GetInt("Game_GameChat", _defaultGameChat);
            FieldOfView.Value = PlayerPrefs.GetFloat("Game_FieldOfView", _defaultFieldOfView);

            SetHasChanges(false);
        }

        public override void ApplySettings()
        {
            // Сохранение настроек в PlayerPrefs
            PlayerPrefs.SetInt("Game_InterfaceMode", InterfaceMode.Value);
            PlayerPrefs.SetInt("Game_GameChat", GameChat.Value);
            PlayerPrefs.SetFloat("Game_FieldOfView", FieldOfView.Value);
            
            // Применение настроек к игре
            // TODO: Добавить реальную логику применения настроек игры
            ApplyInterfaceSettings();
            ApplyFieldOfView();
            
            SetHasChanges(false);
        }

        private void ApplyInterfaceSettings()
        {
            // Логика применения настроек интерфейса
            // Зависит от конкретной реализации UI менеджера в игре
        }

        private void ApplyFieldOfView()
        {
            // Логика применения поля зрения
            // Примерный код для камеры:
            // Camera.main.fieldOfView = FieldOfView.Value;
        }

        public override void ResetToDefault()
        {
            // Сброс настроек к значениям по умолчанию
            InterfaceMode.Value = _defaultInterfaceMode;
            GameChat.Value = _defaultGameChat;
            FieldOfView.Value = _defaultFieldOfView;

            SetHasChanges(true);
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