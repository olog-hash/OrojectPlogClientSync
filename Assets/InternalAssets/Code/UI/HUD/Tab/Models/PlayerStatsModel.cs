using System;
using R3;

namespace ProjectOlog.Code.UI.HUD.Tab.Models
{
    /// <summary>
    /// Модель для хранения и обновления статистики локального игрока.
    /// Содержит информацию об уровне, опыте и прогрессе игрока.
    /// </summary>
    public class PlayerStatsModel : IDisposable
    {
        public ReactiveProperty<int> PlayerLevel { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> PlayerCurrentExperience { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> PlayerNextLevelExperience { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> PlayerExperienceGained { get; } = new ReactiveProperty<int>();

        public void SetLocalPlayerStats(int level, int experience, int experienceGained, int experienceLeft)
        {
            PlayerLevel.Value = level;
            PlayerCurrentExperience.Value = experience;
            PlayerExperienceGained.Value = experienceGained;
            PlayerNextLevelExperience.Value = experienceLeft;
        }
        
        public void Dispose()
        {
            PlayerLevel.Dispose();
            PlayerCurrentExperience.Dispose();
            PlayerExperienceGained.Dispose();
            PlayerNextLevelExperience.Dispose();
        }
    }
}