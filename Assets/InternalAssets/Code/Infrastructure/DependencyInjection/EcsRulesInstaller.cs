using ProjectOlog.Code.Battle.ECS.Rules;
using ProjectOlog.Code.Network.Profiles.Entities;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class EcsRulesInstaller : MonoInstaller
    {
        [SerializeField] private EcsRules _ecsRules;

        private NetworkEntitiesContainer _entitiesContainer;

        [Inject]
        public void Construct(NetworkEntitiesContainer entitiesContainer)
        {
            _entitiesContainer = entitiesContainer;
        }
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<EcsRules>().FromInstance(_ecsRules).AsSingle().NonLazy();

            _entitiesContainer.RegisterEcsRules(_ecsRules);
        }
    }
}