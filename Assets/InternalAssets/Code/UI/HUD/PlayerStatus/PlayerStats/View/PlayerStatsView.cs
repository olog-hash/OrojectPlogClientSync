using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.HUD.PlayerStats.Presenter;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.PlayerStats.View
{
    public class PlayerStatsView : UIToolkitScreen<PlayerStatsViewModel>
    {
        // Контроллеры индикаторов
        private IndicatorController _healthIndicator;
        private IndicatorController _armorIndicator;

        protected override void SetVisualElements()
        {
            var healthElement = _root.Q<VisualElement>("health-indicator");
            var armorElement = _root.Q<VisualElement>("armor-indicator");

            if (healthElement != null)
            {
                _healthIndicator = new IndicatorController(healthElement);
            }

            if (armorElement != null)
            {
                _armorIndicator = new IndicatorController(armorElement);
            }
        }

        protected override void OnBind(PlayerStatsViewModel model)
        {
            var healthArmorModel = model.HealthArmorModel;
            
            // Устанавливаем начальные максимальные значения
            if (_healthIndicator != null)
            {
                _healthIndicator.SetMaxValue(healthArmorModel.MaxHealth.Value);
            }
            
            if (_armorIndicator != null)
            {
                _armorIndicator.SetMaxValue(healthArmorModel.MaxArmor.Value);
            }
            
            // Подписываемся на изменения здоровья
            healthArmorModel.Health
                .Subscribe(health => {
                    if (_healthIndicator != null)
                        _healthIndicator.SetValue(health);
                })
                .AddTo(_disposables);
                
            healthArmorModel.MaxHealth
                .Subscribe(maxHealth => {
                    if (_healthIndicator != null) {
                        _healthIndicator.SetMaxValue(maxHealth);
                        _healthIndicator.SetValue(healthArmorModel.Health.Value); // Обновляем текущее значение
                    }
                })
                .AddTo(_disposables);
            
            // Подписываемся на изменения брони
            healthArmorModel.Armor
                .Subscribe(armor => {
                    if (_armorIndicator != null)
                        _armorIndicator.SetValue(armor);
                })
                .AddTo(_disposables);
                
            healthArmorModel.MaxArmor
                .Subscribe(maxArmor => {
                    if (_armorIndicator != null) {
                        _armorIndicator.SetMaxValue(maxArmor);
                        _armorIndicator.SetValue(healthArmorModel.Armor.Value); // Обновляем текущее значение
                    }
                })
                .AddTo(_disposables);
        }
        
        protected override void OnUnbind(PlayerStatsViewModel model)
        {
            // При необходимости можно добавить логику отвязки
        }
    }
}