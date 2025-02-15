using ProjectOlog.Code.UI.Menus.LoginMenu;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class LoginInstaller : MonoInstaller
    {
        [SerializeField]
        private ClientConnectionUI _clientConnectionUI;

        public override void InstallBindings()
        {
            var connectionUIInstance = Container.InstantiatePrefabForComponent<ClientConnectionUI>(_clientConnectionUI);

            Container.Bind<ClientConnectionUI>().FromComponentInNewPrefab(connectionUIInstance).AsCached();
        }
    }
}