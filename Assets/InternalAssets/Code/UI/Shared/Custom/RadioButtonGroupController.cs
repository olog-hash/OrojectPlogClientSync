using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Custom
{
     public class RadioButtonGroupController : UIToolkitElementView
    {
        private VisualElement _container;
        private RadioButtonGroup _radioGroup;
        private ReactiveProperty<int> _selectedIndex = new ReactiveProperty<int>(0);
        
        public enum Orientation
        {
            Horizontal,
            Vertical
        }
        
        public RadioButtonGroupController(VisualElement root) : base(root) { }
        
        protected override void SetVisualElements()
        {
            _container = Root;
            _radioGroup = _container as RadioButtonGroup;
            
            // Если контейнер не RadioButtonGroup, логируем ошибку
            if (_radioGroup == null)
            {
                return;
            }
            
            // Добавляем базовый класс для контейнера радиокнопок к родителю
            var parent = _radioGroup.parent;
            if (parent != null && !parent.ClassListContains("custom-radiobutton-group"))
            {
                parent.AddToClassList("custom-radiobutton-group");
            }
        }
        
        /// <summary>
        /// Связывает контроллер с реактивным свойством индекса и массивом опций
        /// </summary>
        public void Bind(ReactiveProperty<int> selectedIndex, string[] options, Orientation orientation = Orientation.Horizontal)
        {
            // Проверка на корректность контейнера
            if (_radioGroup == null)
            {
                return;
            }
            
            // Отвязываем предыдущие события
            UnregisterCallbacks();
            
            // Устанавливаем ориентацию через родительский элемент
            if (_radioGroup.parent != null)
            {
                _radioGroup.parent.EnableInClassList("vertical", orientation == Orientation.Vertical);
            }
            
            // Устанавливаем варианты выбора
            _radioGroup.choices = new List<string>(options);
            
            // Регистрируем обработчик изменения выбора
            _radioGroup.RegisterValueChangedCallback(evt => {
                if (evt.newValue != _selectedIndex.Value)
                {
                    _selectedIndex.Value = evt.newValue;
                }
            });
            
            // Двусторонняя связь с внешним индексом
            selectedIndex
                .Subscribe(newIndex => {
                    if (_selectedIndex.Value != newIndex && newIndex >= 0 && newIndex < options.Length)
                    {
                        _selectedIndex.Value = newIndex;
                    }
                })
                .AddTo(_disposables);
                
            _selectedIndex
                .Subscribe(newIndex => {
                    // Обновляем UI
                    UpdateSelection(newIndex);
                    
                    // Обновляем внешний индекс
                    if (selectedIndex.Value != newIndex)
                    {
                        selectedIndex.Value = newIndex;
                    }
                })
                .AddTo(_disposables);
                
            // Устанавливаем начальное значение
            _selectedIndex.Value = selectedIndex.Value >= 0 && selectedIndex.Value < options.Length 
                ? selectedIndex.Value 
                : 0;
        }
        
        /// <summary>
        /// Обновляет выбор радиокнопки на основе индекса
        /// </summary>
        private void UpdateSelection(int index)
        {
            if (_radioGroup == null || index < 0 || index >= _radioGroup.choices.Count())
                return;
                
            // Обновляем значение без вызова события
            if (_radioGroup.value != index)
            {
                _radioGroup.SetValueWithoutNotify(index);
            }
        }
        
        /// <summary>
        /// Отменяет регистрацию обработчиков событий
        /// </summary>
        private void UnregisterCallbacks()
        {
            if (_radioGroup != null)
            {
                // Удаляем все обработчики, предоставив пустую анонимную функцию
                _radioGroup.UnregisterValueChangedCallback(evt => { });
            }
        }
        
        public override void Dispose()
        {
            base.Dispose();
            UnregisterCallbacks();
            _selectedIndex?.Dispose();
        }
    }
}