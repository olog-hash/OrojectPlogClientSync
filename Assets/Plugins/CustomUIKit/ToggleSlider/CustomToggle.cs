using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomToggle : VisualElement
{
    // CSS классы из .uss файла
    public static readonly string containerUssClassName = "toggle-container";
    public static readonly string sliderUssClassName = "slider-toggle";
    public static readonly string trackUssClassName = "track";
    public static readonly string knobUssClassName = "knob";

    // Внутренние элементы
    private VisualElement _container;
    private Toggle _toggle;
    private VisualElement _track;
    private VisualElement _knob;

    // Событие изменения значения
    public event System.Action<bool> OnValueChanged;

    // Свойство Value
    private bool _value;
    public bool Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                _toggle.value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    // Конструктор
    public CustomToggle()
    {
        // Создаем контейнер
        _container = new VisualElement();
        _container.name = "toggle-container";
        _container.AddToClassList(containerUssClassName);
        hierarchy.Add(_container);

        // Создаем Toggle
        _toggle = new Toggle();
        _toggle.name = "slider-toggle";
        _toggle.AddToClassList(sliderUssClassName);
        _container.Add(_toggle);

        // Создаем track
        _track = new VisualElement();
        _track.name = "track";
        _track.AddToClassList(trackUssClassName);
        _toggle.Add(_track);

        // Создаем knob
        _knob = new VisualElement();
        _knob.name = "knob";
        _knob.AddToClassList(knobUssClassName);
        _track.Add(_knob);

        // Регистрируем обработчик изменения значения
        _toggle.RegisterValueChangedCallback(evt =>
        {
            _value = evt.newValue;
            OnValueChanged?.Invoke(_value);
        });
    }

    // Фабрика для редактора
    public new class UxmlFactory : UxmlFactory<CustomToggle, UxmlTraits> { }

    // Трейты для поддержки атрибутов в UXML
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        private readonly UxmlBoolAttributeDescription _valueAttribute = 
            new UxmlBoolAttributeDescription { name = "value", defaultValue = false };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            CustomToggle customToggle = ve as CustomToggle;
            customToggle.Value = _valueAttribute.GetValueFromBag(bag, cc);
        }
    }
}