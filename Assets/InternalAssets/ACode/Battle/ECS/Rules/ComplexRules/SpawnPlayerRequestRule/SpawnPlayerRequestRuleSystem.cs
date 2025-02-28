using System;
using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code._InDevs.Players.Core.Markers;
using ProjectOlog.Code.Input.Controls;
using ProjectOlog.Code.Mechanics.Impact.Victims;
using ProjectOlog.Code.Mechanics.Mortality.Death;
using ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Players;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Systems;
using Unity.IL2CPP.CompilerServices;
using Unity.VisualScripting;

namespace ProjectOlog.Code.Gameplay.ECS.Rules.GameRules.FirstAppeared.Spawn
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class SpawnPlayerRequestRuleSystem : UpdateSystem
    {
        private Filter _ruleFilter;
        private Filter _localPlayerDeathFilter;
        private PlayerNetworker _playerNetworker;
        private LocalPlayerMonitoring _localPlayerMonitoring;

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

            UpdateCooldown(ref spawnRule, deltaTime);

            // Активация правила при смерти локального игрока
            if (HasLocalPlayerDeathEvent())
            {
                ActivateSpawnRule(ref spawnRule);
            }

            // Проверка возможности респавна
            if (CanProcessSpawnRequest(spawnRule))
            {
                SendSpawnRequest();
            }
        }

        private bool HasLocalPlayerDeathEvent()
        {
            foreach (var deathEntity in _localPlayerDeathFilter)
            {
                ref var entityVictimEvent = ref deathEntity.GetComponent<EntityVictimEvent>();
                
                if (entityVictimEvent.VictimEntity == null) continue;

                if (entityVictimEvent.VictimEntity.Has<LocalPlayerMarker>())
                {
                    return true;
                }
            }

            return false;
        }

        private void UpdateCooldown(ref SpawnRequestRule spawnRule, float deltaTime)
        {
            if (spawnRule.Cooldown > 0)
            {
                spawnRule.Cooldown = Math.Max(0, spawnRule.Cooldown - deltaTime);
            }
        }

        private void ActivateSpawnRule(ref SpawnRequestRule spawnRule)
        {
            spawnRule.Cooldown = SpawnRequestRule.MaxCooldown;
        }

        private bool CanProcessSpawnRequest(SpawnRequestRule spawnRule)
        {
            return InputControls.GetKeyDown(KeyType.Fire) &&
                   _localPlayerMonitoring.IsDead() &&
                   spawnRule.Cooldown == 0;
        }

        private void SendSpawnRequest()
        {
            _playerNetworker.SpawnPlayerRequest();
        }
    }
}