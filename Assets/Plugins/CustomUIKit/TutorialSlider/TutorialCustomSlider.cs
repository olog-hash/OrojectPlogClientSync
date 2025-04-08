using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialCustomSlider : VisualElement
{
    // Фабрика для UXML интеграции
    public new class UxmlFactory : UxmlFactory<TutorialCustomSlider, UxmlTraits> { }
    
    // Определяем кастомные атрибуты для UXML
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlFloatAttributeDescription m_Value = new UxmlFloatAttributeDescription { name = "value", defaultValue = 0f };
        UxmlFloatAttributeDescription m_HighValue = new UxmlFloatAttributeDescription { name = "high-value", defaultValue = 100f };
        
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            var slider = ve as TutorialCustomSlider;
            
            slider.value = m_Value.GetValueFromBag(bag, cc);
            slider.highValue = m_HighValue.GetValueFromBag(bag, cc);
        }
    }
    
    private Slider _slider;
    private VisualElement _dragger;
    private VisualElement _bar;
    private VisualElement _newDragger;
    
    // Свойства
    private float _newDraggerOffset = 0f;
    
    // Свойства для публичного API
    public float value
    {
        get => _slider?.value ?? 0f;
        set { if (_slider != null) _slider.value = value; }
    }
    
    public float highValue
    {
        get => _slider?.highValue ?? 100f;
        set { if (_slider != null) _slider.highValue = value; }
    }
    
    // Вместо Start используем конструктор
    public TutorialCustomSlider()
    {
        // Загружаем и добавляем UXML шаблон
        var template = Resources.Load<VisualTreeAsset>("TutorialSlider");
        if (template != null)
        {
            template.CloneTree(this);
        }
        else
        {
            // Создаем элементы вручную если шаблон не найден
            AddToClassList("tutorial-slider");
            
            _slider = new Slider(0, 100) { value = 42 };
            _slider.AddToClassList("tutorial-slider");
            _slider.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
            _slider.style.height = 80;
            
            Add(_slider);
        }
        
        // Инициализация после создания
        RegisterCallback<GeometryChangedEvent>(OnFirstGeometryChange);
    }
    
    private void OnFirstGeometryChange(GeometryChangedEvent evt)
    {
        // Выполняется один раз, когда элемент добавлен к DOM
        UnregisterCallback<GeometryChangedEvent>(OnFirstGeometryChange);
        
        _slider = this.Q<Slider>() ?? _slider;
        _dragger = _slider.Q<VisualElement>("unity-dragger");
        
        AddElements();
        RegisterCallbacks();
    }
    
    private void AddElements()
    {
        _bar = new VisualElement();
        _bar.name = "Bar";
        _bar.AddToClassList("bar");
        _dragger.Add(_bar);

        _newDragger = new VisualElement();
        _newDragger.name = "NewDragger";
        _newDragger.AddToClassList("new-dragger");
        _newDragger.pickingMode = PickingMode.Ignore;
        _slider.Add(_newDragger);
    }

    private void RegisterCallbacks()
    {
        _slider.RegisterCallback<ChangeEvent<float>>(OnSliderValueChanged);
        _slider.RegisterCallback<GeometryChangedEvent>(OnSliderGeometryChanged);
    }

    private void OnSliderGeometryChanged(GeometryChangedEvent evt)
    {
        UpdateDraggerPosition();
    }

    private void OnSliderValueChanged(ChangeEvent<float> evt)
    {
        UpdateDraggerPosition();
    }
    
    private void UpdateDraggerPosition()
    {
        var distant = new Vector2((_newDragger.layout.width - _dragger.layout.width) / 2 - _newDraggerOffset,
            (_newDragger.layout.height - _dragger.layout.height) / 2 - _newDraggerOffset);
        var position = _dragger.parent.LocalToWorld(_dragger.transform.position);
        
        _newDragger.transform.position = _newDragger.parent.WorldToLocal(position - distant);
    }
}
