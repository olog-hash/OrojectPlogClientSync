using ProjectOlog.Code.Gameplay.Battle;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class BattleFactoryInstaller : MonoInstaller
    {
        [SerializeField]
        private BattleContentFactory _battleFactory;

        public override void InstallBindings()
        {
            Container.Bind<BattleContentFactory>().FromInstance(_battleFactory).AsSingle();
        }
    }
}
