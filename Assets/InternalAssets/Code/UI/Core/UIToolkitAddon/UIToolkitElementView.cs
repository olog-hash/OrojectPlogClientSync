using System;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Core.UIToolkitAddon
{
    /// <summary>
    /// Элемент UI для UIToolkit, позволяющий работать с VisualElement как с единым объектом.
    /// </summary>
    public abstract class UIToolkitElementView : IDisposable
    {
        public bool HideOnAwake { get; protected set; }
        public VisualElement Root { get; protected set; }
        public bool IsHidden => Root?.style.display == DisplayStyle.None;

        protected CompositeDisposable _disposables = new CompositeDisposable();
        
        public UIToolkitElementView() { }

        public UIToolkitElementView(VisualElement root)
        {
            Initialize(root);
        }

        public virtual void Initialize(VisualElement root)
        {
            Root = root ?? throw new ArgumentNullException(nameof(root));

            SetVisualElements();
            
            if (HideOnAwake)
            {
                Hide();
            }
        }
        
        /// <summary>
        /// Устанавливает дочерние визуальные элементы для UI.
        /// Переопределяется внутри каждого наследного класса опционально.
        /// </summary>
        protected virtual void SetVisualElements() { }

        public virtual void Show() => ApplyVisibility(true);
        public virtual void Hide() => ApplyVisibility(false);

        protected void ApplyVisibility(bool isVisible)
        {
            if (Root != null)
            {
                Root.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }

        public virtual void Dispose() => _disposables.Dispose();
    }
}