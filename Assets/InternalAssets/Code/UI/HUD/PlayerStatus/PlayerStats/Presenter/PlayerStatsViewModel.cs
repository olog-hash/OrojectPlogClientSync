using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.PlayerStatus.PlayerStats.Models;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.PlayerStats.Presenter
{
    /// <summary>
    /// Модель-представление статистики игрока.
    /// Координирует отображение и обновление различных показателей состояния игрока,
    /// включая здоровье и броню. Служит связующим звеном между моделями данных
    /// игрока и их визуальным представлением в интерфейсе.
    /// </summary>
    public class PlayerStatsViewModel : BaseViewModel
    {
        public HealthArmorModel HealthArmorModel { get; } = new HealthArmorModel();
        
        public override void Dispose()
        {
            HealthArmorModel.Dispose();
            
            base.Dispose();
        }
    }
}