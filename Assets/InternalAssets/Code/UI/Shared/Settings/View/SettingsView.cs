using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using ProjectOlog.Code.UI.Shared.Settings.View.Controllers;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Settings.View
{
    public class SettingsView : UIToolkitScreen<SettingsViewModel>
    {
        // Радиокнопки для вкладок
        private RadioButton _gameTab;
        private RadioButton _controlsTab;
        private RadioButton _graphicsTab;
        private RadioButton _audioTab;

        // Контенты вкладок
        private VisualElement _content;
        private VisualElement _gameContent;
        private VisualElement _controlsContent;
        private VisualElement _graphicsContent;
        private VisualElement _audioContent;

        // Контроллеры для вкладок
        private GameTabController _gameTabController;
        private ControlsTabController _controlsTabController;
        private GraphicsTabController _graphicsTabController;
        private AudioTabController _audioTabController;

        // Кнопки внизу панели
        private Button _applyButton;
        private Button _closeButton;

        // Фоновый оверлей
        private VisualElement _background;

        protected override void SetVisualElements()
        {
            // Находим все необходимые элементы UI
            _gameTab = _root.Q<RadioButton>("gameTab");
            _controlsTab = _root.Q<RadioButton>("controlsTab");
            _graphicsTab = _root.Q<RadioButton>("graphicsTab");
            _audioTab = _root.Q<RadioButton>("audioTab");

            _content = _root.Q<VisualElement>("content");
            _gameContent = _content.Q<VisualElement>("game-content");
            _controlsContent = _content.Q<VisualElement>("controls-content");
            _graphicsContent = _content.Q<VisualElement>("graphics-content");
            _audioContent = _content.Q<VisualElement>("audio-content");

            // Создаем контроллеры вкладок
            _gameTabController = new GameTabController(_gameContent);
            _controlsTabController = new ControlsTabController(_controlsContent);
            _graphicsTabController = new GraphicsTabController(_graphicsContent);
            _audioTabController = new AudioTabController(_audioContent);

            _applyButton = _root.Q<Button>("applyButton");
            _closeButton = _root.Q<Button>("closeButton");
            _background = _root.Q<VisualElement>("background");
        }

        protected override void RegisterButtonCallbacks()
        {
            // Регистрируем обработчики событий
            _gameTab.RegisterValueChangedCallback(OnGameTabValueChanged);
            _controlsTab.RegisterValueChangedCallback(OnControlsTabValueChanged);
            _graphicsTab.RegisterValueChangedCallback(OnGraphicsTabValueChanged);
            _audioTab.RegisterValueChangedCallback(OnAudioTabValueChanged);

            _applyButton.clicked += OnApplyButtonClicked;
            _closeButton.clicked += OnCloseButtonClicked;
        }

        private void OnGameTabValueChanged(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
                _model.SetActiveTab(SettingsViewModel.ESettingsTab.Game);
        }

        private void OnControlsTabValueChanged(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
                _model.SetActiveTab(SettingsViewModel.ESettingsTab.Controls);
        }

        private void OnGraphicsTabValueChanged(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
                _model.SetActiveTab(SettingsViewModel.ESettingsTab.Graphics);
        }

        private void OnAudioTabValueChanged(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
                _model.SetActiveTab(SettingsViewModel.ESettingsTab.Audio);
        }

        private void OnApplyButtonClicked()
        {
            _model.ApplyActiveTabSettings();
        }

        private void OnCloseButtonClicked()
        {
            _model.CloseSettingsPanel();
        }

        protected override void OnBind(SettingsViewModel model)
        {
            // Установим HideOnAwake для скрытия экрана после инициализации
            HideOnAwake = true;
            
            // Связываем контроллеры вкладок с моделями
            _gameTabController.Bind(model.GameSettings);
            _controlsTabController.Bind(model.ControlsSettings);
            _graphicsTabController.Bind(model.GraphicsSettings);
            _audioTabController.Bind(model.AudioSettings);

            // Подписываемся на изменения
            model.ActiveTab
                .Subscribe(UpdateActiveTab)
                .AddTo(_disposables);

            model.IsApplyButtonActive
                .Subscribe(UpdateApplyButtonState)
                .AddTo(_disposables);

            // Устанавливаем начальные состояния
            UpdateActiveTab(model.ActiveTab.CurrentValue);
            UpdateApplyButtonState(model.IsApplyButtonActive.CurrentValue);
        }

        private void UpdateActiveTab(SettingsViewModel.ESettingsTab tab)
        {
            // Обновляем состояние радиокнопок
            _gameTab.value = tab == SettingsViewModel.ESettingsTab.Game;
            _controlsTab.value = tab == SettingsViewModel.ESettingsTab.Controls;
            _graphicsTab.value = tab == SettingsViewModel.ESettingsTab.Graphics;
            _audioTab.value = tab == SettingsViewModel.ESettingsTab.Audio;

            // Обновляем видимость контентов
            _gameContent.style.display =
                tab == SettingsViewModel.ESettingsTab.Game ? DisplayStyle.Flex : DisplayStyle.None;
            _controlsContent.style.display =
                tab == SettingsViewModel.ESettingsTab.Controls ? DisplayStyle.Flex : DisplayStyle.None;
            _graphicsContent.style.display =
                tab == SettingsViewModel.ESettingsTab.Graphics ? DisplayStyle.Flex : DisplayStyle.None;
            _audioContent.style.display =
                tab == SettingsViewModel.ESettingsTab.Audio ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void UpdateApplyButtonState(bool isActive)
        {
            _applyButton.SetEnabled(isActive);

            if (isActive)
                _applyButton.RemoveFromClassList("button-disabled");
            else
                _applyButton.AddToClassList("button-disabled");
        }

        protected override void OnUnbind(SettingsViewModel model)
        {
            // Отвязываем контроллеры вкладок
            _gameTabController.Dispose();
            _controlsTabController.Dispose();
            _graphicsTabController.Dispose();
            _audioTabController.Dispose();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Отписываемся от всех событий
            _gameTab?.UnregisterValueChangedCallback(OnGameTabValueChanged);
            _controlsTab?.UnregisterValueChangedCallback(OnControlsTabValueChanged);
            _graphicsTab?.UnregisterValueChangedCallback(OnGraphicsTabValueChanged);
            _audioTab?.UnregisterValueChangedCallback(OnAudioTabValueChanged);

            if (_applyButton != null) _applyButton.clicked -= OnApplyButtonClicked;
            if (_closeButton != null) _closeButton.clicked -= OnCloseButtonClicked;
        }
    }
}