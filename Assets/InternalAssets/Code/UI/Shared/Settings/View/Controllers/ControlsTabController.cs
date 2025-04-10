using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.Shared.Custom;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Settings.View.Controllers
{
    public class ControlsTabController : UIToolkitElementView
    {
        // Контроллеры для основных настроек
        private EnhancedSliderController _mouseSenseSlider;
        private RadioButtonGroupController _mouseInverseRadioGroup;
        private RadioButtonGroupController _mouseModeRadioGroup;

        // Контроллеры для клавиш передвижения
        private KeyBindButtonController _moveForwardBind;
        private KeyBindButtonController _moveBackwardBind;
        private KeyBindButtonController _moveLeftBind;
        private KeyBindButtonController _moveRightBind;
        private KeyBindButtonController _crouchBind;
        private KeyBindButtonController _jumpBind;
        private KeyBindButtonController _slowBind;
        
        // Контроллеры для клавиш оружия
        private KeyBindButtonController _fireBind;
        private KeyBindButtonController _aimBind;
        private KeyBindButtonController _reloadBind;
        private KeyBindButtonController _previousWeaponBind;
        
        // Контроллеры для слотов оружия
        private KeyBindButtonController _weapon1Bind;
        private KeyBindButtonController _weapon2Bind;
        private KeyBindButtonController _weapon3Bind;
        private KeyBindButtonController _weapon4Bind;
        private KeyBindButtonController _weapon5Bind;
        private KeyBindButtonController _weapon6Bind;
        private KeyBindButtonController _weapon7Bind;
        
        // Контроллеры для насмешек и скриншота
        private KeyBindButtonController _taunt1Bind;
        private KeyBindButtonController _taunt2Bind;
        private KeyBindButtonController _taunt3Bind;
        private KeyBindButtonController _screenshotBind;
        
        private Button _defaultSettingsButton;
        
        private ControlsSettingsModel _model;
    
        public ControlsTabController(VisualElement root) : base(root) { }

        protected override void SetVisualElements()
        {
            // Получение UI-элементов
            var mouseSense = Root.Q<VisualElement>("mouse-sense");
            var mouseInverse = Root.Q<VisualElement>("mouse-inverse");
            var mouseMode = Root.Q<VisualElement>("mouse-mode");
            
            _defaultSettingsButton = Root.Q<Button>("defaultSettings");
            
            // Создание контроллеров для основных настроек мыши
            _mouseSenseSlider = new EnhancedSliderController(mouseSense.Q("Slider"));
            _mouseSenseSlider.SetValueFieldVisible(true);
            
            _mouseInverseRadioGroup = new RadioButtonGroupController(mouseInverse.Q("RadioButtonGroup"));
            _mouseModeRadioGroup = new RadioButtonGroupController(mouseMode.Q("RadioButtonGroup"));
            
            // Поиск и инициализация всех кнопок привязки клавиш в ScrollView
            var scrollView = Root.Q<ScrollView>("ScrollView");
            if (scrollView != null)
            {
                // Клавиши передвижения
                var wBlock = scrollView.Q<VisualElement>("w-block");
                var aBlock = scrollView.Q<VisualElement>("a-block");
                var sBlock = scrollView.Q<VisualElement>("s-block");  // Проверяем наличие в XML
                var dBlock = scrollView.Q<VisualElement>("d-block");
                var ctrlBlock = scrollView.Q<VisualElement>("ctrl-block");
                var spaceBlock = scrollView.Q<VisualElement>("space-block");
                var shiftBlock = scrollView.Q<VisualElement>("shift-block");
                
                // Клавиши оружия
                var fireBlock = scrollView.Q<VisualElement>("fire-block");
                var aimBlock = scrollView.Q<VisualElement>("aim-block");
                var reloadBlock = scrollView.Q<VisualElement>("reload-block");
                var qBlock = scrollView.Q<VisualElement>("q-block");
                
                // Слоты оружия
                var w1Block = scrollView.Q<VisualElement>("w1-block");
                var w2Block = scrollView.Q<VisualElement>("w2-block");
                var w3Block = scrollView.Q<VisualElement>("w3-block");
                var w4Block = scrollView.Q<VisualElement>("w4-block");
                var w5Block = scrollView.Q<VisualElement>("w5-block");
                var w6Block = scrollView.Q<VisualElement>("w6-block");
                var w7Block = scrollView.Q<VisualElement>("w7-block");
                
                // Насмешки и скриншот
                var t1Block = scrollView.Q<VisualElement>("t1-block");
                var t2Block = scrollView.Q<VisualElement>("t2-block");
                var t3Block = scrollView.Q<VisualElement>("t3-block");
                var pBlock = scrollView.Q<VisualElement>("p-block");
                
                // Инициализация контроллеров для клавиш
                _moveForwardBind = InitKeyBindController(wBlock);
                _moveLeftBind = InitKeyBindController(aBlock);
                _moveBackwardBind = InitKeyBindController(sBlock ?? aBlock); // Обработка возможного отсутствия
                _moveRightBind = InitKeyBindController(dBlock);
                _crouchBind = InitKeyBindController(ctrlBlock);
                _jumpBind = InitKeyBindController(spaceBlock);
                _slowBind = InitKeyBindController(shiftBlock);
                
                _fireBind = InitKeyBindController(fireBlock);
                _aimBind = InitKeyBindController(aimBlock);
                _reloadBind = InitKeyBindController(reloadBlock);
                _previousWeaponBind = InitKeyBindController(qBlock);
                
                _weapon1Bind = InitKeyBindController(w1Block);
                _weapon2Bind = InitKeyBindController(w2Block);
                _weapon3Bind = InitKeyBindController(w3Block);
                _weapon4Bind = InitKeyBindController(w4Block);
                _weapon5Bind = InitKeyBindController(w5Block);
                _weapon6Bind = InitKeyBindController(w6Block);
                _weapon7Bind = InitKeyBindController(w7Block);
                
                _taunt1Bind = InitKeyBindController(t1Block);
                _taunt2Bind = InitKeyBindController(t2Block);
                _taunt3Bind = InitKeyBindController(t3Block);
                _screenshotBind = InitKeyBindController(pBlock);
            }
        }
        
        // Вспомогательный метод для инициализации KeyBindButtonController
        private KeyBindButtonController InitKeyBindController(VisualElement container)
        {
            if (container == null) return null;
            
            var button = container.Q<Button>("Button");
            return button != null ? new KeyBindButtonController(button) : null;
        }

        protected override void RegisterButtonCallbacks()
        {
            _defaultSettingsButton.clicked += OnDefaultSettingsButtonClicked;
        }

        public void Bind(ControlsSettingsModel model)
        {
            _model = model;
            
            // Привязка контроллеров основных настроек
            _mouseInverseRadioGroup?.Bind(_model.MouseInverse, _model.MouseInverseOptions);
            _mouseModeRadioGroup?.Bind(_model.MouseMode, _model.MouseModeOptions);
            _mouseSenseSlider?.Bind(_model.MouseSensitivity, 0, 100);
            
            // Привязка контроллеров клавиш
            BindKeyControllers();
        }

        private void BindKeyControllers()
        {
            // Привязка клавиш передвижения
            _moveForwardBind?.Bind(_model.MoveForward);
            _moveBackwardBind?.Bind(_model.MoveBackward);
            _moveLeftBind?.Bind(_model.MoveLeft);
            _moveRightBind?.Bind(_model.MoveRight);
            _crouchBind?.Bind(_model.Crouch);
            _jumpBind?.Bind(_model.Jump);
            _slowBind?.Bind(_model.Slow);
            
            // Привязка клавиш оружия
            _fireBind?.Bind(_model.Fire);
            _aimBind?.Bind(_model.Aim);
            _reloadBind?.Bind(_model.Reload);
            _previousWeaponBind?.Bind(_model.PreviousWeapon);
            
            // Привязка слотов оружия
            _weapon1Bind?.Bind(_model.Weapon1);
            _weapon2Bind?.Bind(_model.Weapon2);
            _weapon3Bind?.Bind(_model.Weapon3);
            _weapon4Bind?.Bind(_model.Weapon4);
            _weapon5Bind?.Bind(_model.Weapon5);
            _weapon6Bind?.Bind(_model.Weapon6);
            _weapon7Bind?.Bind(_model.Weapon7);
            
            // Привязка насмешек и скриншота
            _taunt1Bind?.Bind(_model.Taunt1);
            _taunt2Bind?.Bind(_model.Taunt2);
            _taunt3Bind?.Bind(_model.Taunt3);
            _screenshotBind?.Bind(_model.Screenshot);
        }

        private void OnDefaultSettingsButtonClicked()
        {
            _model.ResetToDefault();
        }
        
        public override void Dispose()
        {
            base.Dispose();
            
            // Отписка от событий
            if (_defaultSettingsButton != null)
                _defaultSettingsButton.clicked -= OnDefaultSettingsButtonClicked;
            
            // Освобождение ресурсов контроллеров
            _mouseInverseRadioGroup?.Dispose();
            _mouseModeRadioGroup?.Dispose();
            _mouseSenseSlider?.Dispose();
            
            DisposeKeyControllers();
        }
        
        private void DisposeKeyControllers()
        {
            // Освобождение ресурсов контроллеров клавиш
            _moveForwardBind?.Dispose();
            _moveBackwardBind?.Dispose();
            _moveLeftBind?.Dispose();
            _moveRightBind?.Dispose();
            _crouchBind?.Dispose();
            _jumpBind?.Dispose();
            _slowBind?.Dispose();
            
            _fireBind?.Dispose();
            _aimBind?.Dispose();
            _reloadBind?.Dispose();
            _previousWeaponBind?.Dispose();
            
            _weapon1Bind?.Dispose();
            _weapon2Bind?.Dispose();
            _weapon3Bind?.Dispose();
            _weapon4Bind?.Dispose();
            _weapon5Bind?.Dispose();
            _weapon6Bind?.Dispose();
            _weapon7Bind?.Dispose();
            
            _taunt1Bind?.Dispose();
            _taunt2Bind?.Dispose();
            _taunt3Bind?.Dispose();
            _screenshotBind?.Dispose();
        }
    }
}