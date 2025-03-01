using ProjectOlog.Code.UI.Core;
using TMPro;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.CrossHair.InteractionPanel
{
    public class InteractionView: AbstractScreen<InteractionViewModel>
    {
        [SerializeField] private TextMeshProUGUI ObjectName;
        [SerializeField] private TextMeshProUGUI ObjectAction;
        [SerializeField] private TextMeshProUGUI ObjectDescription;
        
        private InteractionViewModel _currentViewModel;
        
        protected override void OnBind(InteractionViewModel model)
        {
            OnShowHideChanged(false);
            
            _currentViewModel = model;
            _currentViewModel.OnShowHideChanged += OnShowHideChanged;
            _currentViewModel.OnUpdateData += OnUpdateData;
        }

        protected override void OnUnbind(InteractionViewModel model)
        {
            _currentViewModel.OnShowHideChanged -= OnShowHideChanged;
            _currentViewModel.OnUpdateData -= OnUpdateData;
        }
        
        private void OnShowHideChanged(bool isShown)
        {
            gameObject.SetActive(isShown);
        }

        private void OnUpdateData()
        {
            ObjectName.text = _currentViewModel.InteractionObjectName;
            ObjectAction.text = _currentViewModel.InteractionActionName;
            ObjectDescription.text = _currentViewModel.InteractionObjectDescription;
        }
    }
}