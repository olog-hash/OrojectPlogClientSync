using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code.UI.HUD.Tab.Models;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.Tab.View.Services
{
    public class TeamHeightCalculator
    {
        private const float MIN_TEAM_HEIGHT = 70f;
        private const float DEFAULT_PLAYER_SLOT_HEIGHT = 24f;
        private const float PADDING = 10f;

        public Dictionary<int, float> CalculateTeamHeights(
            IList<TeamModel> teams,
            Dictionary<int, VisualElement> teamElements,
            float totalAvailableHeight)
        {
            if (teams == null || teams.Count == 0 || totalAvailableHeight <= 0)
                return new Dictionary<int, float>();

            var result = new Dictionary<int, float>();
            
            // Шаг 1: Базовое равномерное распределение
            float equalShare = totalAvailableHeight / teams.Count;
            
            // Создаём словари для хранения данных по командам
            var minRequiredHeights = new Dictionary<int, float>();
            var idealHeights = new Dictionary<int, float>();
            
            // Шаг 2: Определяем минимальную и идеальную высоту для каждой команды
            foreach (var team in teams)
            {
                int teamId = team.TeamID.Value;
                int playerCount = team.Players.Count;
                
                if (!teamElements.TryGetValue(teamId, out var teamElement))
                    continue;
                
                // Минимальная высота (заголовки + хотя бы 1 игрок)
                float minHeight = CalculateMinHeight(teamElement);
                minRequiredHeights[teamId] = minHeight;
                
                // Идеальная высота (для отображения всех игроков)
                float idealHeight = CalculateIdealHeight(teamElement, playerCount);
                idealHeights[teamId] = idealHeight;
                
                // Изначально даём равную долю всем командам
                result[teamId] = equalShare;
            }
            
            // Шаг 3: Проверка, нужно ли кому-то больше места
            var teamsNeedingMore = idealHeights
                .Where(kv => kv.Value > equalShare)
                .OrderByDescending(kv => kv.Value)
                .ToList();
            
            var teamsWithExtraSpace = idealHeights
                .Where(kv => kv.Value < equalShare)
                .OrderBy(kv => kv.Value)
                .ToList();
            
            // Если есть команды, которым не хватает места
            if (teamsNeedingMore.Count > 0)
            {
                // Сколько дополнительного места нужно
                float extraNeeded = teamsNeedingMore.Sum(kv => kv.Value - equalShare);
                
                // Сколько можно взять от команд с запасом
                float extraAvailable = teamsWithExtraSpace.Sum(kv => equalShare - kv.Value);
                
                // Если можем взять место у других команд
                if (extraAvailable > 0)
                {
                    // Коэффициент перераспределения (сколько можем удовлетворить)
                    float redistributionRatio = Mathf.Min(1f, extraAvailable / extraNeeded);
                    
                    // Забираем у тех, у кого есть лишнее
                    float totalTaken = 0f;
                    
                    foreach (var kv in teamsWithExtraSpace)
                    {
                        int teamId = kv.Key;
                        float idealHeight = kv.Value;
                        float minHeight = minRequiredHeights[teamId];
                        
                        // Сколько можно сжать эту команду (не меньше минимальной высоты)
                        float canShrink = equalShare - idealHeight;
                        float actualShrink = canShrink * redistributionRatio;
                        
                        // Уменьшаем до минимальной высоты, если нужно
                        if (equalShare - actualShrink < minHeight)
                            actualShrink = equalShare - minHeight;
                        
                        result[teamId] = equalShare - actualShrink;
                        totalTaken += actualShrink;
                    }
                    
                    // Распределяем забранное место между командами, которым нужно больше
                    float totalGiven = 0f;
                    
                    foreach (var kv in teamsNeedingMore.Take(teamsNeedingMore.Count - 1))  // Кроме последней
                    {
                        int teamId = kv.Key;
                        float idealHeight = kv.Value;
                        
                        // Сколько дополнительного места нужно
                        float needsExtra = idealHeight - equalShare;
                        float actualExtra = needsExtra * redistributionRatio;
                        
                        result[teamId] = equalShare + actualExtra;
                        totalGiven += actualExtra;
                    }
                    
                    // Последнюю команду корректируем, чтобы общая сумма точно равнялась totalAvailableHeight
                    if (teamsNeedingMore.Count > 0)
                    {
                        int lastTeamId = teamsNeedingMore.Last().Key;
                        result[lastTeamId] = equalShare + (totalTaken - totalGiven);
                    }
                }
            }
            
            // Шаг 4: Корректируем сумму, чтобы точно соответствовать доступной высоте
            float totalHeight = result.Values.Sum();
            if (!Mathf.Approximately(totalHeight, totalAvailableHeight))
            {
                var lastTeamId = result.Keys.Last();
                result[lastTeamId] += (totalAvailableHeight - totalHeight);
            }
            
            return result;
        }
        
        private float CalculateMinHeight(VisualElement teamElement)
        {
            // Минимальная высота команды (заголовки + минимум для списка)
            var teamHeader = teamElement.Q<VisualElement>("TeamHeader");
            var playersHeader = teamElement.Q<VisualElement>("PlayersHeader");
            
            float headerHeight = teamHeader != null ? teamHeader.resolvedStyle.height : 35f;
            float playersHeaderHeight = playersHeader != null ? playersHeader.resolvedStyle.height : 20f;
            
            return Mathf.Max(MIN_TEAM_HEIGHT, headerHeight + playersHeaderHeight + PADDING);
        }
        
        private float CalculateIdealHeight(VisualElement teamElement, int playerCount)
        {
            if (playerCount == 0)
                return CalculateMinHeight(teamElement);
                
            var teamHeader = teamElement.Q<VisualElement>("TeamHeader");
            var playersHeader = teamElement.Q<VisualElement>("PlayersHeader");
            
            float headerHeight = teamHeader != null ? teamHeader.resolvedStyle.height : 35f;
            float playersHeaderHeight = playersHeader != null ? playersHeader.resolvedStyle.height : 20f;
            
            var playerListView = teamElement.Q<ListView>("PlayerListView");
            float slotHeight = DEFAULT_PLAYER_SLOT_HEIGHT;
            
            if (playerListView != null && playerListView.fixedItemHeight > 0)
            {
                slotHeight = playerListView.fixedItemHeight;
            }
            else if (playerListView != null && playerListView.resolvedItemHeight > 0)
            {
                slotHeight = playerListView.resolvedItemHeight;
            }
            
            float playersHeight = playerCount * slotHeight;
            
            return headerHeight + playersHeaderHeight + playersHeight + PADDING;
        }
    }
}