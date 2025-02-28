using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.Shared;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.HealthPanel
{
    public class HealthView : AbstractScreen<HealthViewModel>, IShowView
    {
        [SerializeField] private TeamIndicator _healthIndicator;
        [SerializeField] private TeamIndicator _armorIndicator;
        
        private HealthViewModel _currentViewModel;
        
        protected override void OnBind(HealthViewModel model)
        {
            _currentViewModel = model;

            _currentViewModel.OnShowHideChanged += OnShowHideChanged;
            _currentViewModel.OnHealthValueChanged += OnHealthValueChanged;
            _currentViewModel.OnArmorValueChanged += OnArmorValueChanged;
        }

        protected override void OnUnbind(HealthViewModel model)
        {
            _currentViewModel.OnShowHideChanged -= OnShowHideChanged;
            _currentViewModel.OnHealthValueChanged -= OnHealthValueChanged;
            _currentViewModel.OnArmorValueChanged -= OnArmorValueChanged;
        }

        private void OnHealthValueChanged()
        {
            _healthIndicator.UpdateIndicator(_currentViewModel.MaxHealth, _currentViewModel.CurrentHealth);
        }

        private void OnArmorValueChanged()
        {
            _armorIndicator.UpdateIndicator(_currentViewModel.MaxArmor, _currentViewModel.CurrentArmor);
        }

        public void OnShowHideChanged(bool isShown)
        {
            gameObject.SetActive(isShown);
        }
    }
}
