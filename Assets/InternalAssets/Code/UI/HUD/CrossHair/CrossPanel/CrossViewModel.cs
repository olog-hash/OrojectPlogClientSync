using System;
using ProjectOlog.Code.UI.Core;

namespace ProjectOlog.Code.UI.HUD.CrossHair.CrossPanel
{
    public class CrossViewModel : BaseViewModel
    {
        public bool IsTargetOnAim
        {
            get => _isTargetOnAim;
            set
            {
                if (_isTargetOnAim != value)
                {
                    _isTargetOnAim = value;
                    OnUpdateData?.Invoke();
                }
            }
        }

        private bool _isTargetOnAim;
        
        
        public Action OnUpdateData;
        
        public CrossViewModel()
        {
            _isTargetOnAim = false;
            OnUpdateData = null;
        }
    }
}