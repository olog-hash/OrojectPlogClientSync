using ProjectOlog.Code.UI.Core;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.AmmoPanel
{
    public class AmmoView : AbstractScreen<AmmoViewModel>
    {
        private AmmoViewModel _currentViewModel;
        
        protected override void OnBind(AmmoViewModel model)
        {
            _currentViewModel = model;
        }

        protected override void OnUnbind(AmmoViewModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}