using ProjectOlog.Code.UI.Core;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.PlayerStats.Presenter
{
    public class PlayerStatsViewModel : BaseViewModel
    {
        // Реактивные свойства для здоровья и брони
        public ReactiveProperty<float> Health { get; } = new ReactiveProperty<float>(100f);
        public ReactiveProperty<float> MaxHealth { get; } = new ReactiveProperty<float>(100f);
        
        public ReactiveProperty<float> Armor { get; } = new ReactiveProperty<float>(100f);
        public ReactiveProperty<float> MaxArmor { get; } = new ReactiveProperty<float>(100f);
        
        // Методы для изменения значений здоровья и брони
        public void SetHealth(float value)
        {
            Health.Value = Mathf.Clamp(value, 0, MaxHealth.Value);
        }
        
        public void SetArmor(float value)
        {
            Armor.Value = Mathf.Clamp(value, 0, MaxArmor.Value);
        }
        
        public override void Dispose()
        {
            Health.Dispose();
            MaxHealth.Dispose();
            Armor.Dispose();
            MaxArmor.Dispose();
            
            base.Dispose();
        }
    }
}