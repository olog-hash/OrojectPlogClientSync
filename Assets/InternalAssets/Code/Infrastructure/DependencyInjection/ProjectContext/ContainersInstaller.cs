using System.Linq;
using ProjectOlog.Code.Context;
using ProjectOlog.Code.DataStorage.Core;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class ContainersInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ContainersFactory>().AsSingle().NonLazy();
            
            BindAllImplementationsOf<IProjectContainer>();
            BindAllImplementationsOf<ISceneContainer>();
            
            Container.Bind<ContainersReloadService>().AsSingle().NonLazy();
            Container.Bind<ContextLifeCycleService>().AsSingle();
        }
        
        public void BindAllImplementationsOf<TInterface>() where TInterface : class
        {
            // Регистрация конкретных классов
            Container.Bind(x => x.AllTypes()
                    .DerivingFrom<TInterface>()
                    .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition))
                .ToSelf()
                .AsSingle();
        }
    }
}