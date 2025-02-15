using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class BattleHUDInstaller : MonoInstaller
    {
        [SerializeField]
        private HUDInitializator _hudInitializator;

        public override void InstallBindings()
        {
            Container.Bind<InterfaceBindLogic>().AsSingle().NonLazy();
            Container.Bind<HUDFactory>().AsSingle().NonLazy();
            Container.Bind<NotificationFactory>().AsSingle().NonLazy();
            
            // Все ViewModel'ы 
            Container.Bind(x => x.AllTypes() 
                    .DerivingFrom<BaseViewModel>() 
                    .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)) // исключая абстрактные классы и определения обобщенных типов
                .ToSelf() // привязать как их собственные типы
                .AsSingle(); 
            
            // Все Notification'ы 
            Container.Bind(x => x.AllTypes() 
                    .DerivingFrom<AbstractNotification>() 
                    .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition)) // исключая абстрактные классы и определения обобщенных типов
                .ToSelf() // привязать как их собственные типы
                .AsTransient(); 
            
            var hudInitializator = Container.InstantiatePrefabForComponent<HUDInitializator>(_hudInitializator);
            
            Container.Bind<HUDInitializator>().FromInstance(hudInitializator).AsCached();
        }
    }
}