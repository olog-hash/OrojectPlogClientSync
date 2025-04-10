using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.Shared.Custom;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Settings.View.Controllers
{
    public class AudioTabController : UIToolkitElementView
    {
        // Контроллеры элементов управления
        private RadioButtonGroupController _muteAllRadioGroup;
        private EnhancedSliderController _masterVolumeSlider;
        private EnhancedSliderController _musicVolumeSlider;
        private EnhancedSliderController _effectsVolumeSlider;
        
        private Button _defaultSettingsButton;
        
        private AudioSettingsModel _model;
    
        public AudioTabController(VisualElement root) : base(root) { }
        
        protected override void SetVisualElements()
        {
            // Получение UI-элементов
            var toggleVolumeElement = Root.Q<VisualElement>("toggle-volume");
            var globalVolumeElement = Root.Q<VisualElement>("global-volume");
            var musicVolumeElement = Root.Q<VisualElement>("music-volume");
            var effectsVolumeElement = Root.Q<VisualElement>("effects-volume");
            
            _defaultSettingsButton = Root.Q<Button>("defaultSettings");
            
            // Создание контроллеров
            _muteAllRadioGroup = new RadioButtonGroupController(toggleVolumeElement.Q("RadioButtonGroup"));
            _masterVolumeSlider = new EnhancedSliderController(globalVolumeElement.Q("Slider"));
            _musicVolumeSlider = new EnhancedSliderController(musicVolumeElement.Q("Slider"));
            _effectsVolumeSlider = new EnhancedSliderController(effectsVolumeElement.Q("Slider"));
            
            // Настройка слайдеров для отображения значений
            _masterVolumeSlider.SetValueFieldVisible(true);
            _musicVolumeSlider.SetValueFieldVisible(true);
            _effectsVolumeSlider.SetValueFieldVisible(true);
        }
        
        protected override void RegisterButtonCallbacks()
        {
            _defaultSettingsButton.clicked += OnDefaultSettingsButtonClicked;
        }
    
        public void Bind(AudioSettingsModel model)
        {
            _model = model;
            
            // Привязка контроллеров к модели
            _muteAllRadioGroup?.Bind(_model.MuteAll, _model.MuteOptions);
            _masterVolumeSlider?.Bind(_model.MasterVolume, 0, 100);
            _musicVolumeSlider?.Bind(_model.MusicVolume, 0, 100);
            _effectsVolumeSlider?.Bind(_model.EffectsVolume, 0, 100);
        }
        
        private void OnDefaultSettingsButtonClicked()
        {
            _model.ResetToDefault();
        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            // Очистка контроллеров
            _muteAllRadioGroup?.Dispose();
            _masterVolumeSlider?.Dispose();
            _musicVolumeSlider?.Dispose();
            _effectsVolumeSlider?.Dispose();
            
            if (_defaultSettingsButton != null)
                _defaultSettingsButton.clicked -= OnDefaultSettingsButtonClicked;
        }
    }
}