using ProjectOlog.Code._InDevs.CameraSystem.Game.ViewControllers.CameraViews;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.CameraExtended.CamerasLogic
{
    public class SpectatorCameraService
    {
        private MainBattleCamera _thirdPersonCamera;

        public SpectatorCameraService(MainBattleCamera thirdPersonCamera) 
        {
            _thirdPersonCamera = thirdPersonCamera;
        }

        public void SwitchToThirdPerson(ChangePersonViewController controller)
        {
            if (controller.ThirdPersonPivot is null) return;
            
            SetTarget(controller.ThirdPersonPivot);
            SetZoomAbility(true, true);
            SetCameraActive(controller.FirstPersonPivot, true);
        }
        
        public void SetTarget(Transform followTransform)
        {
            _thirdPersonCamera.SetTarget(followTransform);
            _thirdPersonCamera.OrbitCamera.SetFollowTransform(followTransform);
        }

        public void SetCameraActive(Transform firstPersonCameraTransform, bool flag)
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
            float maxZoom = isZoomAvaliable ? 5f : 0f;
            
            _thirdPersonCamera.OrbitCamera.MaxDistance = maxZoom;
            
            if (isLunched && _thirdPersonCamera.OrbitCamera.TargetDistance < 0.1f)
            {
                _thirdPersonCamera.OrbitCamera.SetCurrentDistance(maxZoom / 2);
            }
        }
    }
}