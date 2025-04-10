using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Custom
{
    public class EnhancedSliderController : UIToolkitElementView
    {
        // Базовый SliderController для связи с данными
        private SliderController _dataController;
        
        // UI элементы
        private VisualElement _track;
        private VisualElement _fillTrack;
        private VisualElement _dragger;
        private TextField _valueField;
        
        // Параметры слайдера
        private float _min = 0;
        private float _max = 1;
        private bool _hasFocus = false;
        
        // Настройки текстового поля
        private bool _showValueField = false;
        private bool _valueFieldEditable = false;
        private int _decimalPlaces = 0; // По умолчанию целые числа
        
        // Значение для внутреннего использования
        private ReactiveProperty<float> _internalValue = new ReactiveProperty<float>(0);
        
        public EnhancedSliderController(VisualElement root) : base(root) { }
        
        protected override void SetVisualElements()
        {
            // 1. Очищаем корневой элемент
            Root.Clear();
            
            // 2. Добавляем основной класс для слайдера
            Root.AddToClassList("enhanced-slider");
            
            // 3. Создаем структуру слайдера
            CreateSliderElements();
            
            // 4. Настраиваем обработку взаимодействия
            SetupInteractions();
            
            // 5. Создаем скрытый элемент для SliderController
            var hiddenSlider = new VisualElement();
            hiddenSlider.style.display = DisplayStyle.None;
            hiddenSlider.Add(new Slider());
            Root.Add(hiddenSlider);
            
            // 6. Инициализируем контроллер данных
            _dataController = new SliderController(hiddenSlider);
            
            // 7. Подписываемся на изменения внутреннего значения
            _internalValue
                .Subscribe(value => {
                    UpdateVisualFromValue(value);
                    UpdateValueField(value);
                })
                .AddTo(_disposables);
        }
        
        private void CreateSliderElements()
        {
            // Создаем трек
            _track = new VisualElement();
            _track.name = "Track";
            _track.AddToClassList("enhanced-slider__track");
            Root.Add(_track);
            
            // Создаем заполнение трека
            _fillTrack = new VisualElement();
            _fillTrack.name = "Fill";
            _fillTrack.AddToClassList("enhanced-slider__fill");
            _track.Add(_fillTrack);
            
            // Создаем ползунок
            _dragger = new VisualElement();
            _dragger.name = "Dragger";
            _dragger.AddToClassList("enhanced-slider__dragger");
            _dragger.focusable = true;
            _track.Add(_dragger);
            
            // Создаем текстовое поле для отображения значения
            _valueField = new TextField();
            _valueField.name = "ValueField";
            _valueField.AddToClassList("enhanced-slider__value-field");
            _valueField.style.display = _showValueField ? DisplayStyle.Flex : DisplayStyle.None;
            _valueField.isReadOnly = !_valueFieldEditable;
            
            // Если не редактируемое, отключаем интерактивность полностью
            if (!_valueFieldEditable)
            {
                _valueField.pickingMode = PickingMode.Ignore;
                _valueField.focusable = false;
            }
            
            // Добавляем поле к корневому элементу
            Root.Add(_valueField);
        }
        
        private void SetupInteractions()
        {
            // Обработка клика по треку
            _track.RegisterCallback<PointerDownEvent>(evt => {
                SetValueFromPointerPosition(evt.localPosition);
                _track.CapturePointer(evt.pointerId);
                
                _dragger.Focus();
                _hasFocus = true;
            });
            
            // Обработка перемещения указателя
            _track.RegisterCallback<PointerMoveEvent>(evt => {
                if (_track.HasPointerCapture(evt.pointerId)) {
                    SetValueFromPointerPosition(evt.localPosition);
                }
            });
            
            // Освобождение указателя
            _track.RegisterCallback<PointerUpEvent>(evt => {
                if (_track.HasPointerCapture(evt.pointerId)) {
                    _track.ReleasePointer(evt.pointerId);
                }
            });
            
            // Обработка геометрических изменений
            _track.RegisterCallback<GeometryChangedEvent>(evt => {
                UpdateVisualFromValue(_internalValue.Value);
            });
            
            // Обработка клавиш для доступности
            _dragger.RegisterCallback<KeyDownEvent>(evt => {
                switch (evt.keyCode) {
                    case KeyCode.LeftArrow:
                        DecreaseValue();
                        evt.StopPropagation();
                        break;
                    case KeyCode.RightArrow:
                        IncreaseValue();
                        evt.StopPropagation();
                        break;
                }
            });
            
            // Обработка изменения значения в текстовом поле (если оно редактируемое)
            if (_valueFieldEditable)
            {
                _valueField.RegisterValueChangedCallback(evt => {
                    if (float.TryParse(evt.newValue, out float newValue)) {
                        newValue = Mathf.Clamp(newValue, _min, _max);
                        _internalValue.Value = newValue;
                    }
                });
            }
        }
        
        private void UpdateValueField(float value)
        {
            if (_valueField == null) return;
            
            // Форматируем значение в зависимости от количества знаков после запятой
            string formattedValue = _decimalPlaces > 0 
                ? value.ToString($"F{_decimalPlaces}") 
                : ((int)value).ToString();
            
            // Обновляем текст только если он отличается
            if (_valueField.value != formattedValue) {
                _valueField.value = formattedValue;
            }
        }
        
        private void IncreaseValue(float step = 0.1f)
        {
            float range = _max - _min;
            float stepValue = range * step;
            _internalValue.Value = Mathf.Min(_max, _internalValue.Value + stepValue);
        }
        
        private void DecreaseValue(float step = 0.1f)
        {
            float range = _max - _min;
            float stepValue = range * step;
            _internalValue.Value = Mathf.Max(_min, _internalValue.Value - stepValue);
        }
        
        private void SetValueFromPointerPosition(Vector2 position)
        {
            float trackWidth = _track.layout.width;
            if (trackWidth <= 0) return;
            
            float percentage = Mathf.Clamp01(position.x / trackWidth);
            float value = _min + percentage * (_max - _min);
            _internalValue.Value = value;
        }
        
        private void UpdateVisualFromValue(float value)
        {
            if (_track == null || _fillTrack == null || _dragger == null) return;
            
            float percentage = (_max - _min) != 0 ? 
                (value - _min) / (_max - _min) : 0;
            
            // Обновляем заполненную часть трека
            _fillTrack.style.width = Length.Percent(percentage * 100);
            
            // Обновляем позицию ползунка
            _dragger.style.left = Length.Percent(percentage * 100);
        }
        
        /// <summary>
        /// Связывает слайдер с реактивным свойством
        /// </summary>
        public void Bind(ReactiveProperty<float> value, float min = 0, float max = 1)
        {
            _min = min;
            _max = max;
            
            // Связываем с внутренним контроллером
            _dataController.Bind(_internalValue, min, max);
            
            // Синхронизируем начальное значение
            _internalValue.Value = value.Value;
            
            // Двусторонняя связь: внутреннее значение -> внешнее значение
            _internalValue
                .Subscribe(newValue => {
                    if (Mathf.Abs(value.Value - newValue) > 0.001f) {
                        value.Value = newValue;
                    }
                })
                .AddTo(_disposables);
            
            // Двусторонняя связь: внешнее значение -> внутреннее значение
            value
                .Subscribe(newValue => {
                    if (Mathf.Abs(_internalValue.Value - newValue) > 0.001f) {
                        _internalValue.Value = newValue;
                    }
                })
                .AddTo(_disposables);
        }
        
        /// <summary>
        /// Устанавливает статус доступности слайдера
        /// </summary>
        public void SetEnabled(bool enabled)
        {
            Root.SetEnabled(enabled);
        }
        
        /// <summary>
        /// Показывает или скрывает текстовое поле со значением
        /// </summary>
        public void SetValueFieldVisible(bool visible)
        {
            _showValueField = visible;
            if (_valueField != null) {
                _valueField.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
        
        /// <summary>
        /// Устанавливает возможность редактирования значения через текстовое поле
        /// </summary>
        public void SetValueFieldEditable(bool editable)
        {
            _valueFieldEditable = editable;
            if (_valueField != null)
            {
                _valueField.isReadOnly = !editable;
                
                // Настройки интерактивности
                if (!editable)
                {
                    // Отключаем выбор элемента
                    _valueField.pickingMode = PickingMode.Ignore;
                    // Запрещаем фокусировку
                    _valueField.focusable = false;
                    
                    // Находим текстовой элемент внутри и тоже делаем его неинтерактивным
                    var textInput = _valueField.Q(className: "unity-text-element") ?? 
                                  _valueField.Q("unity-text-input");
                    if (textInput != null)
                    {
                        textInput.pickingMode = PickingMode.Ignore;
                        textInput.focusable = false;
                    }
                }
                else
                {
                    // Возвращаем интерактивность
                    _valueField.pickingMode = PickingMode.Position;
                    _valueField.focusable = true;
                    
                    // Обновляем настройки внутреннего элемента
                    var textInput = _valueField.Q(className: "unity-text-element") ?? 
                                  _valueField.Q("unity-text-input");
                    if (textInput != null)
                    {
                        textInput.pickingMode = PickingMode.Position;
                        textInput.focusable = true;
                    }
                    
                    // Регистрируем обработчик изменения значения, если его еще нет
                    _valueField.RegisterValueChangedCallback(evt => {
                        if (float.TryParse(evt.newValue, out float newValue)) {
                            newValue = Mathf.Clamp(newValue, _min, _max);
                            _internalValue.Value = newValue;
                        }
                    });
                }
            }
        }
        
        /// <summary>
        /// Устанавливает количество знаков после запятой для отображения
        /// </summary>
        public void SetDecimalPlaces(int places)
        {
            _decimalPlaces = Mathf.Max(0, places);
            UpdateValueField(_internalValue.Value);
        }
        
        /// <summary>
        /// Напрямую устанавливает значение слайдера
        /// </summary>
        public void SetValue(float value)
        {
            _internalValue.Value = Mathf.Clamp(value, _min, _max);
        }
        
        /// <summary>
        /// Получает текущее значение слайдера
        /// </summary>
        public float GetValue()
        {
            return _internalValue.Value;
        }
        
        public override void Dispose()
        {
            base.Dispose();
            _dataController?.Dispose();
            _internalValue?.Dispose();
        }
    }
}