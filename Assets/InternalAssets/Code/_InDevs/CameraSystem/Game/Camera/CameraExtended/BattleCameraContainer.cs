using ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.CameraExtended.CamerasLogic;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.CameraExtended
{
    /// <summary>
    /// Основной хост для главной камеры.
    /// Нужен для того, чтобы при заходе в игру - камера была в ключевой точке и смотрела на бой, пока не был выбран игрок для наблюдения или заспавнен локальный игрок.
    /// </summary>
    public class BattleCameraContainer : MonoBehaviour
    {
        [SerializeField] private Transform _battleMainPoint;
        
        private MainBattleCamera _mainBattleCamera;
        private BattleCameraService _battleCameraService;

        [Inject]
        public void Construct(MainBattleCamera mainBattleCamera)
        {
            _mainBattleCamera = mainBattleCamera;
            _battleCameraService = new BattleCameraService(_mainBattleCamera);
            
            Initialize();
        }

        private void Initialize()
        {
            if (_battleMainPoint == null) return;

            ResetBattlePosition(_battleMainPoint);
        }
        
        public void ResetBattlePosition(Transform battleMainPoint)
        {
            _battleCameraService.SetTarget(battleMainPoint);
            _battleCameraService.SetZoomAbility(false, true);
            _battleCameraService.SetFallowRotation(true);
            _battleCameraService.SetCameraActive(true);
        }
    }
}