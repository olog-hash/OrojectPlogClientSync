using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code._InDevs.Data.Sessions;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class BattleRoomInstaller : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.Bind<LocalInventorySession>().AsSingle().NonLazy();
            Container.Bind<LocalPlayerMonitoring>().AsSingle().NonLazy();
        }
    }
}