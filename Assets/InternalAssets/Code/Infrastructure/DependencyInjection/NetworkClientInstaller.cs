using System.Linq;
using ProjectOlog.Code.Networking.Client;
using ProjectOlog.Code.Networking.Infrastructure.CompoundEvents;
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

            // Распаковщики ивентов
            BindAllEventUnpacker();
            
            // Нетворкеры
            BindAllNetWorkers();
            
            // Системы и все остальное
            Container.Bind<NetworkClientGate>().AsSingle();
            Container.BindInterfacesAndSelfTo<NetworkClient>().FromInstance(_networkClient).AsSingle().NonLazy();
        }

        private void BindAllEventUnpacker()
        {
            Container.Bind(x => x.AllTypes()
                    .DerivingFrom<BasedNetworkUnpacker>()
                    .Where(type => !type.IsAbstract && !type.IsGenericTypeDefinition))
                .ToSelf()
                .AsTransient();
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
    }
}