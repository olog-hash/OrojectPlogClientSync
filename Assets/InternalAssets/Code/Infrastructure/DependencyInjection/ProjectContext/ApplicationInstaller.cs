using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Infrastructure.Application.StateMachine;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class ApplicationInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindApplicationLayerManager();

            BindApplicationStateMachine();
        }

        private void BindApplicationLayerManager()
        {
            Container.Bind<ApplicationLayersController>().AsSingle();
            Container.Bind<LayersManager>().AsSingle();
        }
        
        private void BindApplicationStateMachine()
        {
            Container.Bind<ApplicationStateFactory>().AsSingle().NonLazy();

            Container.Bind(x => x.AllTypes()
                    .DerivingFrom<ApplicationState>() 
                    .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)) // исключая абстрактные классы и определения обобщенных типов
                .ToSelf() // привязать как их собственные типы
                .AsSingle(); // с транзитным временем жизни

            Container.BindInterfacesAndSelfTo<ApplicationStateMachine>().AsSingle();
        }
    }
}