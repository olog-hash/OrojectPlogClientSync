using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code._InDevs.Players.Visual.SpectatorPersonSystem.SpectatorPerson.Switching;
using ProjectOlog.Code.Mechanics.Repercussion.Damage.Core.Death;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.Visual.SpectatorPersonSystem.Rules
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [UpdateBefore((typeof(SpectatorSwitchSystem)))]
    public sealed class DeathCameraTransitionRuleSystem : TickrateSystem
    {
        private Filter _ruleFilter;
        private Filter _deathPlayerFilter;
        private Filter _spectatorSwitchingFilter;
        private LocalPlayerMonitoring _localPlayerMonitoring;

        public DeathCameraTransitionRuleSystem(LocalPlayerMonitoring localPlayerMonitoring)
        {
            _localPlayerMonitoring = localPlayerMonitoring;
        }

        public override void OnAwake()
        {
            _ruleFilter = World.Filter.With<DeathCameraTransitionRule>().Build();
            _deathPlayerFilter = World.Filter.With<DeathEvent>().Build();
            _spectatorSwitchingFilter = World.Filter.With<SpectatorSwitchRequestEvent>().Build(); 
        }

        public override void OnUpdate(float deltaTime)
        {
            ref var deathCameraTransitionRule =
                ref _ruleFilter.First().GetComponent<DeathCameraTransitionRule>(out var ruleExist);
            if (!ruleExist) return;

            // Обработка события смерти
            foreach (var entityEvent in _deathPlayerFilter)
            {
                ref var deathEvent = ref entityEvent.GetComponent<DeathEvent>();
                if (deathEvent.VictimEntity.IsNullOrDisposed()) continue;

                if (deathEvent.VictimEntity.Has<LocalPlayerMarker>())
                {
                    LaunchRule(ref deathCameraTransitionRule);
                    break;
                }
            }

            // Если правило активно, блокируем ручные попытки переключить
            if (deathCameraTransitionRule.IsTriggered)
            {
                // Если обнаружили что был ручной запрос - удаляем его, ибо игрок не может переключать раньше правила.
                if (_spectatorSwitchingFilter.IsNotEmpty())
                {
                    foreach (var spectatorSwitchingEvent in _spectatorSwitchingFilter)
                    {
                        spectatorSwitchingEvent.Dispose();
                    }
                }

                // Если игрок живой - сворачиваемся
                if (!_localPlayerMonitoring.IsDead())
                {
                    DeactivateRule(ref deathCameraTransitionRule);
                }
                
                ProcessTimer(ref deathCameraTransitionRule, deltaTime);
            }
        }

        private void LaunchRule(ref DeathCameraTransitionRule rule)
        {
            rule.IsTriggered = true;
            rule.DelayTimer = DeathCameraTransitionRule.MaxDelayDuration;
        }

        private void DeactivateRule(ref DeathCameraTransitionRule rule)
        {
            rule.IsTriggered = false;
            rule.DelayTimer = 0f;
        }

        private void ProcessTimer(ref DeathCameraTransitionRule rule, float deltaTime)
        {
            rule.DelayTimer = Mathf.Max(0f, rule.DelayTimer - deltaTime);

            if (rule.DelayTimer <= 0f)
            {
                // Сбрасываем правило
                rule.IsTriggered = false;

                // Создаем событие переключения камеры
                World.CreateTickEvent().AddComponent<SpectatorSwitchRequestEvent>();
            }
        }
    }
}