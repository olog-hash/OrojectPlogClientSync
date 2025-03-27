using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Features.Players.Visual.SpectatorPersonSystem.SpectatorPerson.Switching;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code.Battle.ECS.Rules.ComplexRules.DeathCameraTransitionRule
{
    /// <summary>
    /// Система, управляющая переходом камеры после смерти локального игрока.
    /// Предотвращает ручное переключение камеры в течение определенного времени,
    /// а затем автоматически переключает камеру на другого игрока.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [UpdateBefore((typeof(SpectatorSwitchSystem)))]
    public sealed class DeathCameraTransitionRuleSystem : TickrateSystem
    {
        private Filter _ruleFilter;
        private Filter _deathPlayerFilter;
        private Filter _spectatorSwitchingFilter;
        
        private readonly LocalPlayerMonitoring _localPlayerMonitoring;

        public DeathCameraTransitionRuleSystem(LocalPlayerMonitoring localPlayerMonitoring)
        {
            _localPlayerMonitoring = localPlayerMonitoring;
        }

        public override void OnAwake()
        {
            _ruleFilter = World.Filter.With<DeathCameraTransitionRule>().Build();
            _deathPlayerFilter = World.Filter.With<DeathEvent>().With<EntityVictimEvent>().Build();
            _spectatorSwitchingFilter = World.Filter.With<SpectatorSwitchRequestEvent>().Build(); 
        }

       public override void OnUpdate(float deltaTime)
        {
            ref var deathCameraRule =
                ref _ruleFilter.First().GetComponent<DeathCameraTransitionRule>(out var ruleExist);
            if (!ruleExist) return;

            // Обработка события смерти локального игрока
            ProcessLocalPlayerDeathEvents(ref deathCameraRule);

            // Если правило активно, управляем переключением камеры
            if (deathCameraRule.IsActive)
            {
                // Блокируем ручные попытки переключения камеры
                CancelManualCameraSwitchRequests();

                // Если игрок воскрес, деактивируем правило
                if (!_localPlayerMonitoring.IsDead())
                {
                    DeactivateRule(ref deathCameraRule);
                    return;
                }
                
                // Обработка таймера и переключение камеры по истечении задержки
                ProcessRuleTimer(ref deathCameraRule, deltaTime);
            }
        }
       
        // Обрабатывает события смерти и активирует правило, если погиб локальный игрок.
        private void ProcessLocalPlayerDeathEvents(ref DeathCameraTransitionRule rule)
        {
            foreach (var entityEvent in _deathPlayerFilter)
            {
                ref var entityVictimEvent = ref entityEvent.GetComponent<EntityVictimEvent>();
                if (entityVictimEvent.VictimEntity == null || entityVictimEvent.VictimEntity.IsNullOrDisposed()) continue;

                if (entityVictimEvent.VictimEntity.Has<LocalPlayerMarker>())
                {
                    ActivateRule(ref rule);
                    break;
                }
            }
        }
        
        // Отменяет все запросы на ручное переключение камеры.
        private void CancelManualCameraSwitchRequests()
        {
            if (_spectatorSwitchingFilter.IsNotEmpty())
            {
                foreach (var spectatorSwitchingEvent in _spectatorSwitchingFilter)
                {
                    spectatorSwitchingEvent.RuleDispose();
                }
            }
        }
        
        // Активирует правило перехода камеры и устанавливает таймер задержки.
        private void ActivateRule(ref DeathCameraTransitionRule rule)
        {
            rule.IsActive = true;
            rule.RemainingDelay = DeathCameraTransitionRule.MaxDelayDuration;
        }
        
        // Деактивирует правило перехода камеры и сбрасывает таймер.
        private void DeactivateRule(ref DeathCameraTransitionRule rule)
        {
            rule.IsActive = false;
            rule.RemainingDelay = 0f;
        }
        
        // Обрабатывает таймер правила и создает событие переключения камеры по истечении задержки.
        private void ProcessRuleTimer(ref DeathCameraTransitionRule rule, float deltaTime)
        {
            rule.RemainingDelay = Mathf.Max(0f, rule.RemainingDelay - deltaTime);

            if (rule.RemainingDelay <= 0f)
            {
                // Сбрасываем правило
                rule.IsActive = false;

                // Создаем событие переключения камеры
                World.CreateTickEvent().AddComponent<SpectatorSwitchRequestEvent>();
            }
        }
    }
}