using System;
using ProjectOlog.Code.UI.Core;

namespace ProjectOlog.Code.UI.HUD.CrossHair.InteractionPanel
{
    public class InteractionViewModel: BaseViewModel
    {
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnShowHideChanged?.Invoke(value);
                }
            }
        }
        
        public string InteractionObjectName
        {
            get => _interactionObjectName;
            set
            {
                if (InteractionObjectName != value)
                {
                    _interactionObjectName = value;
                    OnUpdateData?.Invoke();
                }
            }
        }
        
        public string InteractionObjectDescription
        {
            get => _interactionObjectDescription;
            set
            {
                if (_interactionObjectDescription != value)
                {
                    _interactionObjectDescription = value;
                    OnUpdateData?.Invoke();
                }
            }
        }
        
        public string InteractionActionName
        {
            get => _interactionActionName;
            set
            {
                if (_interactionActionName != value)
                {
                    _interactionActionName = value;
                    OnUpdateData?.Invoke();
                }
            }
        }
        
        
        
        public Action<bool> OnShowHideChanged;
        public Action OnUpdateData;
        
        private bool _isVisible;
        private string _interactionObjectName;
        private string _interactionObjectDescription;
        private string _interactionActionName;
        
        public InteractionViewModel()
        {
            _isVisible = false;
            OnShowHideChanged = null;
            
            InteractionObjectName = string.Empty;
            InteractionObjectDescription = string.Empty;
            InteractionActionName = string.Empty;
        }

        public void ClearInfoText()
        {
            InteractionObjectName = string.Empty;
            InteractionObjectDescription = string.Empty;
            InteractionActionName = string.Empty;
        }
    }
}