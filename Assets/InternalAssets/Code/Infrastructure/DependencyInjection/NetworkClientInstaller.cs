using System.Linq;
using ProjectOlog.Code.Networking.Client;
using ProjectOlog.Code.Networking.Handlers.ComponentHandlers;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using ProjectOlog.Code.Networking.Profiles.Entities;
using ProjectOlog.Code.Networking.Profiles.Snapshots;
using ProjectOlog.Code.Networking.Profiles.Users;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class NetworkClientInstaller : MonoInstaller
    {
        [SerializeField]
        private NetworkClient _networkClient;

        public override void InstallBindings()
        {
            // Основные данные
            Container.Bind<NetworkUsersContainer>().AsSingle();
            Container.Bind<NetworkEntitiesContainer>().AsSingle();
            Container.Bind<NetworkSnapshotContainer>().AsSingle();
            Container.Bind<NetTransportProvider>().AsSingle();

            // Сериализаторы компонентов
            BindAllComponentSerializators();
            
            // Нетворкеры
            BindAllNetWorkers();
            
            // Системы и все остальное
            Container.Bind<NetworkClientGate>().AsSingle();
            Container.BindInterfacesAndSelfTo<NetworkClient>().FromInstance(_networkClient).AsSingle().NonLazy();
        }
        
        private void BindAllNetWorkers()
        {
            Container.Bind<NetWorkerFactory>().AsSingle();
            
            /*
            Container.Bind(typeof(NetWorkerClient)).To(x => x
                    .AllTypes()
                    .DerivingFrom<NetWorkerClient>()
                    .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition))
                .AsSingle();
            */
            
            Container.Bind(x => x.AllTypes()
                    .DerivingFrom<NetWorkerClient>()
                    .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition))
                .ToSelf()
                .AsTransient();
        }
        
        private void BindAllComponentSerializators()
        {
            Container.Bind<ComponentSerializatorFactory>().AsSingle().NonLazy();
            Container.Bind<ComponentSerializator>().AsSingle().NonLazy();
            
            // Обновленная привязка для обобщенного IComponentSerializer<>
            Container.Bind(x => x.AllTypes()
                    .Where(type => type.GetInterfaces()
                        .Any(i => i.IsGenericType && 
                                  i.GetGenericTypeDefinition() == typeof(IComponentSerializer<>)))
                    .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition))
                .ToSelf()
                .AsSingle();
        }
    }
}