using System;
using ProjectOlog.Code.UI.Core;
using R3;

namespace ProjectOlog.Code.UI.HUD.CrossHair.InteractionPanel
{
    public class InteractionViewModel: BaseViewModel
    {
        // Использование реактивных свойств вместо обычных свойств с событиями
        private ReactiveProperty<string> _interactionObjectName = new ReactiveProperty<string>(string.Empty);
        private ReactiveProperty<string> _interactionObjectDescription = new ReactiveProperty<string>(string.Empty);
        private ReactiveProperty<string> _interactionActionName = new ReactiveProperty<string>(string.Empty);
        
        public ReadOnlyReactiveProperty<string> InteractionObjectName => _interactionObjectName.ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<string> InteractionObjectDescription => _interactionObjectDescription.ToReadOnlyReactiveProperty();
        public ReadOnlyReactiveProperty<string> InteractionActionName => _interactionActionName.ToReadOnlyReactiveProperty();
        
        public InteractionViewModel()
        {
            // Конструктор базового класса уже инициализирует видимость
        }

        public void SetInteractionObjectName(string value)
        {
            _interactionObjectName.Value = value;
        }
        
        public void SetInteractionObjectDescription(string value)
        {
            _interactionObjectDescription.Value = value;
        }
        
        public void SetInteractionActionName(string value)
        {
            _interactionActionName.Value = value;
        }

        public void ClearInfoText()
        {
            _interactionObjectName.Value = string.Empty;
            _interactionObjectDescription.Value = string.Empty;
            _interactionActionName.Value = string.Empty;
        }
    }
}