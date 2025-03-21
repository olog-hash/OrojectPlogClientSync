using System;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.PlayerStats.Models
{
    /// <summary>
    /// Модель для управления показателями здоровья и брони игрока.
    /// Обеспечивает реактивные свойства для текущих и максимальных значений,
    /// а также методы для безопасного изменения этих показателей с учетом ограничений.
    /// </summary>
    public class HealthArmorModel : IDisposable
    {
        public ReactiveProperty<float> Health { get; } = new ReactiveProperty<float>(100f);
        public ReactiveProperty<float> MaxHealth { get; } = new ReactiveProperty<float>(100f);
        
        public ReactiveProperty<float> Armor { get; } = new ReactiveProperty<float>(100f);
        public ReactiveProperty<float> MaxArmor { get; } = new ReactiveProperty<float>(100f);
        
        // Методы для изменения значений здоровья и брони
        public void SetHealth(float value)
        {
            Health.Value = Mathf.Clamp(value, 0, MaxHealth.Value);
        }

        public void SetMaxHealth(float value)
        {
            MaxHealth.Value = value;
        }
        
        public void SetArmor(float value)
        {
            Armor.Value = Mathf.Clamp(value, 0, MaxArmor.Value);
        }
        
        public void SetMaxArmor(float value)
        {
            MaxArmor.Value = value;
        }
        
        public void Dispose()
        {
            Health.Dispose();
            MaxHealth.Dispose();
            Armor.Dispose();
            MaxArmor.Dispose();
        }
    }
}