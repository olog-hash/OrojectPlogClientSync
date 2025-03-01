using ProjectOlog.Code.Engine.Cameras.Types;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.Infrastructure.DependencyInjection
{
    public class MainBattleCameraInstaller : MonoInstaller
    {
        [SerializeField] private BattleCameraContainer _battleCameraContainer;
        [SerializeField] private MainBattleCamera _mainBattleCamera;

        public override void InstallBindings()
        {
            var mainBattleCamera = Container.InstantiatePrefabForComponent<MainBattleCamera>(_mainBattleCamera);
            
            mainBattleCamera.Initialize();
            Container.Bind<MainBattleCamera>().FromInstance(mainBattleCamera).AsCached();
            
            Container.BindInterfacesAndSelfTo<BattleCameraContainer>().FromInstance(_battleCameraContainer).AsSingle().NonLazy();
        }
    }
}