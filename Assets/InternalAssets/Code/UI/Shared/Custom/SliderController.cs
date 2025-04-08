using System;
using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Custom
{
    // Контроллер для Slider
    public class SliderController : UIToolkitElementView
    {
        private Slider _slider;
        private ReactiveProperty<float> _value;
    
        public SliderController(VisualElement root) : base(root) { }
    
        protected override void SetVisualElements()
        {
            _slider = Root.Q<Slider>();
        }
    
        public void Bind(ReactiveProperty<float> value, float min = 0, float max = 1)
        {
            _value = value;
        
            // Настройка границ
            _slider.lowValue = min;
            _slider.highValue = max;
        
            // Начальное значение
            _slider.value = _value.Value;
        
            // Подписка на изменения в UI
            _slider.RegisterValueChangedCallback(evt => 
            {
                if (Math.Abs(_value.Value - evt.newValue) > 0.001f)
                {
                    _value.Value = evt.newValue;
                }
            });
        
            // Подписка на изменения в модели
            _value
                .Subscribe(newValue => 
                {
                    if (Math.Abs(_slider.value - newValue) > 0.001f)
                    {
                        _slider.value = newValue;
                    }
                })
                .AddTo(_disposables);
        }
    }
}