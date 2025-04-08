using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Custom
{
    // Контроллер для ToggleSlider
    public class ToggleController : UIToolkitElementView
    {
        private Toggle _toggle;
        private ReactiveProperty<bool> _value;
    
        public ToggleController(VisualElement root) : base(root) { }
    
        protected override void SetVisualElements()
        {
            _toggle = Root.Q<Toggle>("slider-toggle");
        }
    
        public void Bind(ReactiveProperty<bool> value)
        {
            _value = value;
        
            // Начальное значение
            _toggle.value = _value.Value;
        
            // Подписка на изменения в UI
            _toggle.RegisterValueChangedCallback(evt => 
            {
                if (_value.Value != evt.newValue)
                {
                    _value.Value = evt.newValue;
                }
            });
        
            // Подписка на изменения в модели
            _value
                .Subscribe(newValue => 
                {
                    if (_toggle.value != newValue)
                    {
                        _toggle.value = newValue;
                    }
                })
                .AddTo(_disposables);
        }
    }
}