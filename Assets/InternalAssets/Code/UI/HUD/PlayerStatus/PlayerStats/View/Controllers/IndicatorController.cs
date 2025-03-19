using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.PlayerStats
{
    public class IndicatorController : UIToolkitElementView
    {
        private VisualElement _indicatorMask;
        private Label _valueLabel;
        
        private float _maxValue = 100f;
        private float _currentValue = 100f;
        
        public IndicatorController(VisualElement indicator) : base(indicator)
        {
            
        }

        protected override void SetVisualElements()
        {
            _indicatorMask = Root.Q<VisualElement>("indicator-mask");
            _valueLabel = Root.Q<Label>("indicator-low-value");
            
            // Применяем начальное значение
            UpdateIndicator(_currentValue);
        }

        public void UpdateIndicator(float value)
        {
            _currentValue = Mathf.Clamp(value, 0, _maxValue);
            float percentage = _currentValue / _maxValue;
            
            // Обновляем высоту маски
            if (_indicatorMask != null)
            {
                _indicatorMask.style.height = Length.Percent(percentage * 100);
            }
            
            // Обновляем текст
            if (_valueLabel != null)
            {
                _valueLabel.text = Mathf.RoundToInt(_currentValue).ToString();
            }
        }
        
        public void SetMaxValue(float maxValue)
        {
            _maxValue = Mathf.Max(1f, maxValue); // Предотвращаем деление на ноль
            UpdateIndicator(_currentValue); // Обновляем индикатор с новым максимальным значением
        }
        
        public void SetValue(float value)
        {
            _currentValue = value;
            UpdateIndicator(_currentValue);
        }
    }
}