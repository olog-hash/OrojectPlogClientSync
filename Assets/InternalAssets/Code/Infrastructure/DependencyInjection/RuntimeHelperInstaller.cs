using ProjectOlog.Code.Infrastructure.TimeManagement;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class RuntimeHelperInstaller : MonoInstaller
    {
        [SerializeField]
        private RuntimeHelper _runtimeHelper;

        public override void InstallBindings()
        {
            Container.Bind<RuntimeHelper>().FromInstance(_runtimeHelper).AsSingle();
        }
    }
}