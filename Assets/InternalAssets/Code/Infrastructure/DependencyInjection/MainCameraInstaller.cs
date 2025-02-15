using ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.Core;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class MainCameraInstaller: MonoInstaller
    {
        [SerializeField]
        private MainCamera _mainCamera;

        public override void InstallBindings()
        {
            var mainCamera = Container.InstantiatePrefabForComponent<MainCamera>(_mainCamera);
            
            mainCamera.Initialize();
            
            Container.Bind<MainCamera>().FromInstance(mainCamera).AsCached();
        }
    }
}