using System;
using ProjectOlog.Code.UI.Core;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.HealthPanel
{
    public class HealthViewModel : BaseViewModel, IShowViewModel
    {
        public event Action<bool> OnShowHideChanged;
        public event Action OnHealthValueChanged;
        public event Action OnArmorValueChanged;
    
        private int _maxHealth;
        private int _currentHealth;
        private int _maxArmor;
        private int _currentArmor;
    
        public int MaxHealth
        {
            get => _maxHealth;
            set
            {
                if (_maxHealth != value)
                {
                    _maxHealth = value;
                    OnHealthValueChanged?.Invoke();
                    // Дополнительная логика при изменении MaxHealth
                }
            }
        }
    
        public int CurrentHealth
        {
            get => _currentHealth;
            set
            {
                if (_currentHealth != value)
                {
                    _currentHealth = value;
                    OnHealthValueChanged?.Invoke();
                    // Дополнительная логика при изменении CurrentHealth
                }
            }
        }
    
        public int MaxArmor
        {
            get => _maxArmor;
            set
            {
                if (_maxArmor != value)
                {
                    _maxArmor = value;
                    OnArmorValueChanged?.Invoke();
                    // Дополнительная логика при изменении MaxArmor
                }
            }
        }
    
        public int CurrentArmor
        {
            get => _currentArmor;
            set
            {
                if (_currentArmor != value)
                {
                    _currentArmor = value;
                    OnArmorValueChanged?.Invoke();
                    // Дополнительная логика при изменении CurrentArmor
                }
            }
        }
        
        public void OnShow()
        {
            OnShowHideChanged?.Invoke(true);
        }

        public void OnHide()
        {
            OnShowHideChanged?.Invoke(false);
        }
    }
}
