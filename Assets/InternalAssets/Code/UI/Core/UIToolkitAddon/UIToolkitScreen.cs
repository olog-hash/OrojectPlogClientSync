using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Core.UIToolkitAddon
{
    /// <summary>
    /// Специфичный экран для UIToolkit, работающий с UIDocument как с View.
    /// </summary>
    public abstract class UIToolkitScreen<TModel> : AbstractScreen<TModel> where TModel : IViewModel
    {
        protected UIDocument _uiDocument;
        protected VisualElement _root;
        public bool IsHidden => _root?.style.display == DisplayStyle.None;

        public override void Initialize()
        {
            if (_uiDocument == null)
                _uiDocument = GetComponent<UIDocument>();
                
            if (_uiDocument != null)
                _root = _uiDocument.rootVisualElement;
                
            if (_root == null) {
                Debug.LogError($"Failed to initialize UI root for {name}. SetVisualElements will not be called.");
                return; // Добавляем return, чтобы не вызывать SetVisualElements() если _root == null
            }

            SetVisualElements();
            RegisterButtonCallbacks();
        }

        /// <summary>
        /// Устанавливает дочерние визуальные элементы для UI.
        /// Переопределяется внутри каждого наследного класса опционально.
        /// </summary>
        protected virtual void SetVisualElements() { }
        
        /// <summary>
        /// Устанавливает все call-back'и для визуальных элементов для UI
        /// Переопределяется внутри каждого наследного класса опционально.
        /// </summary>
        protected virtual void RegisterButtonCallbacks() { }
        
        // Переопределяем метод, ибо в UIToolkit отображение и сокрытие работают чуть иначе.
        protected override void ApplyVisibility(bool isVisible)
        {
            if (_root != null)
            {
                _root.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}