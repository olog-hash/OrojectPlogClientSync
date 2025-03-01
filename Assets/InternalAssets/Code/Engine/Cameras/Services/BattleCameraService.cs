using ProjectOlog.Code.Engine.Cameras.Types;
using ProjectOlog.Code.Engine.Cameras.ViewControllers;

namespace ProjectOlog.Code.Engine.Cameras.Services
{
    /// <summary>
    /// Сервис для управления основной battle-камерой.
    /// Battle-камера, это самая первая камера которая отображается при подключении на карту.
    /// </summary>
    public class BattleCameraService
    {
        private MainBattleCamera _thirdPersonCamera;

        public BattleCameraService(MainBattleCamera thirdPersonCamera) 
        {
            _thirdPersonCamera = thirdPersonCamera;
        }

        public void SwitchToThirdPerson(ChangePersonViewController controller)
        {
            if (controller.ThirdPersonPivot is null) return;
            
            SetTarget(controller.ThirdPersonPivot);
            SetFallowRotation();
            SetZoomAbility(true, false);
            SetCameraActive(true);
        }
        
        public void SetTarget(UnityEngine.Transform followTransform)
        {
            _thirdPersonCamera.SetTarget(followTransform);
            _thirdPersonCamera.OrbitCamera.SetFollowTransform(followTransform);
        }

        public void SetCameraActive(bool flag)
        {
            _thirdPersonCamera.SetCameraActive(flag);
            
            if (!_thirdPersonCamera.IsActive)
            {
                SetFallowRotation(false);
            }
        }

        public void SetFallowRotation(bool flag = true)
        {
            _thirdPersonCamera.OrbitCamera.SyncWithTargetRotation();
        }

        public void SetZoomAbility(bool isZoomAvaliable, bool isLunched = false)
        {
            float maxZoom = isZoomAvaliable ? 10f : 0f;
            
            _thirdPersonCamera.OrbitCamera.MaxDistance = maxZoom;

            if (isLunched)
            {
                _thirdPersonCamera.OrbitCamera.SetCurrentDistance(maxZoom / 2);
            }
        }
    }
}