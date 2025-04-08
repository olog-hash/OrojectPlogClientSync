using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.UI.Core;
using R3;

namespace ProjectOlog.Code.UI.Shared.Settings.Presenter
{
    public class SettingsViewModel : BaseViewModel, ILayer
    {
        // Добавляем константу имени слоя
        public const string LAYER_NAME = "Settings";
        
        // Модели для каждой вкладки
        public GameSettingsModel GameSettings { get; }
        public ControlsSettingsModel ControlsSettings { get; }
        public GraphicsSettingsModel GraphicsSettings { get; }
        public AudioSettingsModel AudioSettings { get; }

        // Текущая активная вкладка
        private ReactiveProperty<ESettingsTab> _activeTab = new ReactiveProperty<ESettingsTab>(ESettingsTab.Graphics);
        public ReadOnlyReactiveProperty<ESettingsTab> ActiveTab => _activeTab.ToReadOnlyReactiveProperty();

        // Состояние кнопки "Применить"
        private ReactiveProperty<bool> _isApplyButtonActive = new ReactiveProperty<bool>(false);
        public ReadOnlyReactiveProperty<bool> IsApplyButtonActive => _isApplyButtonActive.ToReadOnlyReactiveProperty();

        // Перечисление для вкладок
        public enum ESettingsTab
        {
            Game,
            Controls,
            Graphics,
            Audio
        }
        
        protected override bool IsCanIgnoreGlobalVisibility => true;

        public SettingsViewModel()
        {
            LayersManager.RegisterLayer(3, SettingsViewModel.LAYER_NAME, this, LayerInfo.SelectedPanel);
            
            // Создаем модели для каждой вкладки
            GameSettings = new GameSettingsModel();
            ControlsSettings = new ControlsSettingsModel();
            GraphicsSettings = new GraphicsSettingsModel();
            AudioSettings = new AudioSettingsModel();

            // Подписываемся на изменение активной вкладки
            _activeTab
                .Subscribe(UpdateApplyButtonState)
                .AddTo(_disposables);

            // Подписываемся на изменения в каждой модели
            GameSettings.HasChanges
                .Subscribe(_ => UpdateApplyButtonState(_activeTab.Value))
                .AddTo(_disposables);

            ControlsSettings.HasChanges
                .Subscribe(_ => UpdateApplyButtonState(_activeTab.Value))
                .AddTo(_disposables);

            GraphicsSettings.HasChanges
                .Subscribe(_ => UpdateApplyButtonState(_activeTab.Value))
                .AddTo(_disposables);

            AudioSettings.HasChanges
                .Subscribe(_ => UpdateApplyButtonState(_activeTab.Value))
                .AddTo(_disposables);

            // Начальное обновление состояния кнопки
            UpdateApplyButtonState(_activeTab.Value);
        }

        public void ShowLayer()
        {
            Show();
        }

        public void HideLayer()
        {
            Hide();
        }

        public void CloseSettingsPanel()
        {
            CloseLayer();
        }

        private void CloseLayer()
        {
            if (LayersManager.IsLayerActive(SettingsViewModel.LAYER_NAME))
            {
                LayersManager.HideLayer(SettingsViewModel.LAYER_NAME);
            }
        }
        
        // Переключение активной вкладки
        public void SetActiveTab(ESettingsTab tab)
        {
            _activeTab.Value = tab;
        }

        // Обновление состояния кнопки "Применить"
        private void UpdateApplyButtonState(ESettingsTab tab)
        {
            switch (tab)
            {
                case ESettingsTab.Game:
                    _isApplyButtonActive.Value = GameSettings.HasChanges.CurrentValue;
                    break;
                case ESettingsTab.Controls:
                    _isApplyButtonActive.Value = ControlsSettings.HasChanges.CurrentValue;
                    break;
                case ESettingsTab.Graphics:
                    _isApplyButtonActive.Value = GraphicsSettings.HasChanges.CurrentValue;
                    break;
                case ESettingsTab.Audio:
                    _isApplyButtonActive.Value = AudioSettings.HasChanges.CurrentValue;
                    break;
            }
        }

        // Применение настроек активной вкладки
        public void ApplyActiveTabSettings()
        {
            switch (_activeTab.Value)
            {
                case ESettingsTab.Game:
                    GameSettings.ApplySettings();
                    break;
                case ESettingsTab.Controls:
                    ControlsSettings.ApplySettings();
                    break;
                case ESettingsTab.Graphics:
                    GraphicsSettings.ApplySettings();
                    break;
                case ESettingsTab.Audio:
                    AudioSettings.ApplySettings();
                    break;
            }
        }

        // Сброс настроек активной вкладки
        public void ResetActiveTabToDefault()
        {
            switch (_activeTab.Value)
            {
                case ESettingsTab.Game:
                    GameSettings.ResetToDefault();
                    break;
                case ESettingsTab.Controls:
                    ControlsSettings.ResetToDefault();
                    break;
                case ESettingsTab.Graphics:
                    GraphicsSettings.ResetToDefault();
                    break;
                case ESettingsTab.Audio:
                    AudioSettings.ResetToDefault();
                    break;
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            GameSettings.Dispose();
            ControlsSettings.Dispose();
            GraphicsSettings.Dispose();
            AudioSettings.Dispose();

            _activeTab.Dispose();
            _isApplyButtonActive.Dispose();
        }
    }
}