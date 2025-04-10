using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.Shared.Custom;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Settings.View.Controllers
{
   public class GameTabController : UIToolkitElementView
    {
        // Контроллеры элементов управления
        private DropdownController _interfaceModeDropdown;
        private RadioButtonGroupController _gameChatRadioGroup;
        private EnhancedSliderController _fieldOfViewSlider;
        
        private Button _defaultSettingsButton;
        
        private GameSettingsModel _model;
    
        public GameTabController(VisualElement root) : base(root) { }
        
        protected override void SetVisualElements()
        {
            // Получение UI-элементов
            var interfaceElement = Root.Q<VisualElement>("interface");
            var chatToggleElement = Root.Q<VisualElement>("chat-toggle");
            var fovElement = Root.Q<VisualElement>("fov");
            
            _defaultSettingsButton = Root.Q<Button>("defaultSettings");
            
            // Создание контроллеров
            _interfaceModeDropdown = new DropdownController(interfaceElement.Q<DropdownField>());
            _gameChatRadioGroup = new RadioButtonGroupController(chatToggleElement.Q("RadioButtonGroup"));
            _fieldOfViewSlider = new EnhancedSliderController(fovElement.Q("Slider"));
            
            // Настройка слайдера для отображения значений
            _fieldOfViewSlider.SetValueFieldVisible(true);
        }
        
        protected override void RegisterButtonCallbacks()
        {
            _defaultSettingsButton.clicked += OnDefaultSettingsButtonClicked;
        }
    
        public void Bind(GameSettingsModel model)
        {
            _model = model;
            
            // Привязка контроллеров к модели
            _interfaceModeDropdown?.Bind(_model.InterfaceMode, _model.InterfaceOptions);
            _gameChatRadioGroup?.Bind(_model.GameChat, _model.ChatOptions);
            _fieldOfViewSlider?.Bind(_model.FieldOfView, 60, 120);
        }
        
        private void OnDefaultSettingsButtonClicked()
        {
            _model.ResetToDefault();
        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            // Очистка контроллеров
            _interfaceModeDropdown?.Dispose();
            _gameChatRadioGroup?.Dispose();
            _fieldOfViewSlider?.Dispose();
            
            if (_defaultSettingsButton != null)
                _defaultSettingsButton.clicked -= OnDefaultSettingsButtonClicked;
        }
    }
}