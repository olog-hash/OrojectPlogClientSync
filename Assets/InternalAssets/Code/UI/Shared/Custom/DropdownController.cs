using System;
using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Custom
{
    // Контроллер для выпадающего списка
    public class DropdownController : UIToolkitElementView
    {
        private DropdownField _dropdown;
        private ReactiveProperty<int> _index;
        private string[] _options;
    
        public DropdownController(VisualElement root) : base(root) { }
    
        protected override void SetVisualElements()
        {
            _dropdown = Root.Q<DropdownField>();
        }
    
        public void Bind(ReactiveProperty<int> index, string[] options)
        {
            _index = index;
            _options = options;
        
            _dropdown.choices = new System.Collections.Generic.List<string>(options);
            _dropdown.index = _index.Value;
        
            _dropdown.RegisterValueChangedCallback(evt => 
            {
                int newIndex = Array.IndexOf(_options, evt.newValue);
                if (newIndex >= 0 && _index.Value != newIndex)
                {
                    _index.Value = newIndex;
                }
            });
        
            _index
                .Subscribe(newIndex => 
                {
                    if (_dropdown.index != newIndex && newIndex >= 0 && newIndex < _options.Length)
                    {
                        _dropdown.index = newIndex;
                    }
                })
                .AddTo(_disposables);
        }
    }
}