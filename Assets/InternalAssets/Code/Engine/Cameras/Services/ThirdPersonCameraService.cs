using ProjectOlog.Code.Engine.Cameras.Types;
using ProjectOlog.Code.Engine.Cameras.ViewControllers;

namespace ProjectOlog.Code.Engine.Cameras.Services
{
    /// <summary>
    /// Сервис для управления камерой в режиме от третьего лица.
    /// </summary>
    public class ThirdPersonCameraService
    {
        private MainBattleCamera _thirdPersonCamera;

        public ThirdPersonCameraService(MainBattleCamera thirdPersonCamera) 
        {
            _thirdPersonCamera = thirdPersonCamera;
        }

        /// <summary>
        /// Переключает камеру в режим от третьего лица.
        /// </summary>
        public void SwitchToThirdPerson(ChangePersonViewController controller)
        {
            if (controller.ThirdPersonPivot is null) return;
            
            SetTarget(controller.ThirdPersonPivot);
            SetFallowRotation();
            SetZoomAbility(true, true);
            SetCameraActive(controller.FirstPersonPivot, true);
        }
        
        /// <summary>
        /// Устанавливает целевой объект для следования камеры.
        /// </summary>
        public void SetTarget(UnityEngine.Transform followTransform)
        {
            _thirdPersonCamera.SetTarget(followTransform);
            _thirdPersonCamera.OrbitCamera.SetFollowTransform(followTransform);
        }
        
        public void SetCameraActive(UnityEngine.Transform firstPersonCameraTransform, bool flag)
        {
            _thirdPersonCamera.SetCameraActive(flag);
            
            if (_thirdPersonCamera.IsActive)
            {
                _thirdPersonCamera.OrbitCamera.SetInitialPosition(firstPersonCameraTransform.position, firstPersonCameraTransform.rotation);
            }
            else
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