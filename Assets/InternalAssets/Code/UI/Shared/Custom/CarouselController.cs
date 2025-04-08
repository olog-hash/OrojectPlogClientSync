using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.Shared.Custom
{
    // Контроллер для Carousel
    public class CarouselController : UIToolkitElementView
    {
        private Button _prevButton;
        private Button _nextButton;
        private Label _valueLabel;
    
        private ReactiveProperty<int> _index;
        private string[] _options;
    
        public CarouselController(VisualElement root) : base(root) { }
    
        protected override void SetVisualElements()
        {
            _prevButton = Root.Q<Button>("prev-button");
            _nextButton = Root.Q<Button>("next-button");
            _valueLabel = Root.Q<Label>("value");
        }
    
        protected override void RegisterButtonCallbacks()
        {
            _prevButton.clicked += OnPrevButtonClicked;
            _nextButton.clicked += OnNextButtonClicked;
        }
    
        public void Bind(ReactiveProperty<int> index, string[] options)
        {
            _index = index;
            _options = options;
        
            UpdateValueLabel();
        
            _index
                .Subscribe(_ => UpdateValueLabel())
                .AddTo(_disposables);
        }
    
        private void OnPrevButtonClicked()
        {
            if (_index.Value > 0)
            {
                _index.Value--;
            }
            else
            {
                _index.Value = _options.Length - 1;
            }
        }
    
        private void OnNextButtonClicked()
        {
            if (_index.Value < _options.Length - 1)
            {
                _index.Value++;
            }
            else
            {
                _index.Value = 0;
            }
        }
    
        private void UpdateValueLabel()
        {
            if (_options != null && _index.Value >= 0 && _index.Value < _options.Length)
            {
                _valueLabel.text = _options[_index.Value];
            }
        }
    
        public override void Dispose()
        {
            base.Dispose();
        
            if (_prevButton != null) _prevButton.clicked -= OnPrevButtonClicked;
            if (_nextButton != null) _nextButton.clicked -= OnNextButtonClicked;
        }
    }
}