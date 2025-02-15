using ProjectOlog.Code.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectOlog.Code.UI.HUD.CrossHair.CrossPanel
{
    public class CrossView: AbstractScreen<CrossViewModel>
    {
        [SerializeField] private Image _crossImage;
        
        private CrossViewModel _currentViewModel;
        
        protected override void OnBind(CrossViewModel model)
        {
            _currentViewModel = model;
            
            _currentViewModel.OnUpdateData += OnUpdateData;
        }

        protected override void OnUnbind(CrossViewModel model)
        {
            _currentViewModel.OnUpdateData -= OnUpdateData;
        }
        
        private void OnUpdateData()
        {
            _crossImage.color = _currentViewModel.IsTargetOnAim ? new Color(1f, 0.44f, 0.44f) : Color.white;
        }
    }
}