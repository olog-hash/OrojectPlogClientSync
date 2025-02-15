using ProjectOlog.Code.Gameplay.ECS.Systems;
using Scellecs.Morpeh;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class EcsInstaller : MonoInstaller
    {
        [SerializeField]
        private EcsStartup _ecsStartup;
        
        public override void InstallBindings()
        {
            Container.Bind<EcsSystemsFactory>().AsSingle().NonLazy();

            // Системы 
            Container.Bind(x => x.AllTypes() 
                    .DerivingFrom<ISystem>() 
                    .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)) // исключая абстрактные классы и определения обобщенных типов
                .ToSelf() // привязать как их собственные типы
                .AsTransient(); // с транзитным временем жизни

            // Загрузчик
            Container.BindInterfacesAndSelfTo<EcsStartup>().FromInstance(_ecsStartup).AsSingle().NonLazy();
        }
    }
}