using ProjectOlog.Code.Battle.ECS.Rules.ComplexRules.DeathCameraTransitionRule;
using ProjectOlog.Code.Battle.ECS.Rules.ComplexRules.SpawnPlayerRequestRule;
using ProjectOlog.Code.Engine.Transform;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using UnityEngine;

namespace ProjectOlog.Code.Battle.ECS.Rules
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
                RemainingCooldown = SpawnRequestRule.MaxCooldown,
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
