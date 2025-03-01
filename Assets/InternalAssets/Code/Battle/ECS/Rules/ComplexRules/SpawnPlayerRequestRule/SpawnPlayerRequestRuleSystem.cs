using System;
using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code.Engine.Inputs;
using ProjectOlog.Code.Features.Players.Core.Markers;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Network.Infrastructure.NetWorkers.Entities;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code.Battle.ECS.Rules.ComplexRules.SpawnPlayerRequestRule
{
    /// <summary>
    /// Система, обрабатывающая запросы на возрождение игрока.
    /// Отслеживает смерть локального игрока, управляет кулдауном
    /// и отправляет запрос на возрождение при соответствующих условиях.
    /// </summary>
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SpawnPlayerRequestRuleSystem : UpdateSystem
    {
        private Filter _ruleFilter;
        private Filter _localPlayerDeathFilter;
        
        private readonly PlayerNetworker _playerNetworker;
        private readonly LocalPlayerMonitoring _localPlayerMonitoring;

        public SpawnPlayerRequestRuleSystem(PlayerNetworker playerNetworker,
            LocalPlayerMonitoring localPlayerMonitoring)
        {
            _playerNetworker = playerNetworker;
            _localPlayerMonitoring = localPlayerMonitoring;
        }

        public override void OnAwake()
        {
            _ruleFilter = World.Filter.With<SpawnRequestRule>().Build();
            _localPlayerDeathFilter = World.Filter.With<DeathEvent>().With<EntityVictimEvent>().Build();
        }

        public override void OnUpdate(float deltaTime)
        {
            ref var spawnRule = ref _ruleFilter.First().GetComponent<SpawnRequestRule>(out var ruleExist);
            if (!ruleExist) return;

            // Обновление таймера кулдауна
            UpdateCooldownTimer(ref spawnRule, deltaTime);

            // Активация правила при смерти локального игрока
            if (HasLocalPlayerDeathEvent())
            {
                ActivateCooldown(ref spawnRule);
            }

            // Проверка возможности респавна и отправка запроса
            if (CanRequestSpawn(spawnRule))
            {
                SendSpawnRequest();
            }
        }
        
        // Проверяет, произошла ли смерть локального игрока в текущем кадре.
        private bool HasLocalPlayerDeathEvent()
        {
            foreach (var deathEntity in _localPlayerDeathFilter)
            {
                ref var entityVictimEvent = ref deathEntity.GetComponent<EntityVictimEvent>();
                if (entityVictimEvent.VictimEntity == null || entityVictimEvent.VictimEntity.IsNullOrDisposed()) continue;

                if (entityVictimEvent.VictimEntity.Has<LocalPlayerMarker>())
                {
                    return true;
                }
            }

            return false;
        }
        
        // Обновляет таймер кулдауна, уменьшая его с течением времени.
        private void UpdateCooldownTimer(ref SpawnRequestRule spawnRule, float deltaTime)
        {
            if (spawnRule.RemainingCooldown > 0)
            {
                spawnRule.RemainingCooldown = Math.Max(0, spawnRule.RemainingCooldown - deltaTime);
            }
        }
        
        // Активирует кулдаун возрождения после смерти игрока.
        private void ActivateCooldown(ref SpawnRequestRule spawnRule)
        {
            spawnRule.RemainingCooldown = SpawnRequestRule.MaxCooldown;
        }
        
        // Проверяет, можно ли отправить запрос на возрождение на основе текущего состояния игрока, ввода и кулдауна.
        private bool CanRequestSpawn(SpawnRequestRule spawnRule)
        {
            return InputControls.GetKeyDown(KeyType.Fire) &&
                   _localPlayerMonitoring.IsDead() &&
                   spawnRule.RemainingCooldown == 0;
        }
        
        // Отправляет сетевой запрос на возрождение игрока.
        private void SendSpawnRequest()
        {
            _playerNetworker.SpawnPlayerRequest();
        }
    }
}