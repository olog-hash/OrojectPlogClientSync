using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class IndicatorController : MonoBehaviour
{
    private VisualElement _indicatorContainer;
    private VisualElement _indicatorMask;
    private VisualElement _indicatorBackground;
    private VisualElement _indicatorOutline;
    
    [SerializeField] private float _maxValue = 100f;
    [SerializeField, Range(0, 100)] private float _currentValue = 50f;
    
    [SerializeField] private Texture2D _backgroundTexture;
    [SerializeField] private Texture2D _outlineTexture;
    
    // Для отслеживания изменений в инспекторе
    private float previousHealth;
    
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _indicatorContainer = root.Q<VisualElement>("indicator-container");
        _indicatorMask = root.Q<VisualElement>("indicator-mask");
        _indicatorBackground = root.Q<VisualElement>("indicator-background");
        _indicatorOutline = root.Q<VisualElement>("indicator-outline");
        
        // Устанавливаем текстуру иконки
        if (_backgroundTexture != null)
        {
            _indicatorBackground.style.backgroundImage = new StyleBackground(_backgroundTexture);
        }

        if (_outlineTexture != null)
        {
            _indicatorOutline.style.backgroundImage = new StyleBackground(_outlineTexture);
        }
        
        // Запоминаем начальное значение здоровья
        previousHealth = _currentValue;
        
        // Применяем начальное значение
        UpdateHealthBar(_currentValue);
    }
    
    void Update()
    {
        // Проверяем изменение здоровья в инспекторе
        if (previousHealth != _currentValue)
        {
            UpdateHealthBar(_currentValue);
            previousHealth = _currentValue;
        }
    }
    
    // Этот метод также вызывается в редакторе при изменении значений
    void OnValidate()
    {
        // Убеждаемся, что здоровье в допустимых пределах
        _currentValue = Mathf.Clamp(_currentValue, 0, _maxValue);
    }
    
    public void UpdateHealthBar(float health)
    {
        _currentValue = Mathf.Clamp(health, 0, _maxValue);
        float healthPercentage = _currentValue / _maxValue;
        
        // Обновляем высоту маски только если она существует
        if (_indicatorMask != null)
        {
            _indicatorMask.style.height = Length.Percent(healthPercentage * 100);
        }
    }
    
    public void SetHealth(float health)
    {
        _currentValue = health;
        UpdateHealthBar(_currentValue);
    }
    
    public void SetHealthIcon(Texture2D newTexture)
    {
        if (newTexture != null && _indicatorBackground != null)
        {
            _backgroundTexture = newTexture;
            _indicatorBackground.style.backgroundImage = new StyleBackground(_backgroundTexture);
        }
    }
}