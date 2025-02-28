using ProjectOlog.Code._InDevs.Players.Visual.SpectatorPersonSystem.Rules;
using ProjectOlog.Code.Game.Core;
using ProjectOlog.Code.Gameplay.ECS.Rules.GameRules.FirstAppeared.Spawn;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Gameplay.ECS.Rules
{
    [RequireComponent(typeof(EntityProvider))]
    public class EcsRules : MonoBehaviour
    {
        public Entity Entity => _entityProvider.Entity;
        
        private EntityProvider _entityProvider;
        
        private void Awake()
        {
            _entityProvider = GetComponent<EntityProvider>();

            RegisterBasedRules();
        }

        private void RegisterBasedRules()
        {
            Entity.AddComponent<Translation>();
            Entity.AddComponentData(new SpawnRequestRule
            {
                Cooldown = SpawnRequestRule.MaxCooldown,
            });
            Entity.AddComponentData(new DeathCameraTransitionRule());
            // BattleCamera first
            // Spawn request system
        }

        private void OnDestroy()
        {
            //_entityProvider.AddComponent<RemoveEntityOnDestroy>();
        }
    }
}
