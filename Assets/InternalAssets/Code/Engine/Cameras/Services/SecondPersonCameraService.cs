using ProjectOlog.Code.Engine.Cameras.Types;
using ProjectOlog.Code.Engine.Cameras.ViewControllers;

namespace ProjectOlog.Code.Engine.Cameras.Services
{
    /// <summary>
    /// Сервис для управления камерой в режиме от второго лица.
    /// </summary>
    public class SecondPersonCameraService
    {
        public void SwitchToSecondPerson(ChangePersonViewController controller)
        {
            SetTarget(controller.SecondPersonCamera, controller.SecondPersonPivot);
            SetCameraActive(controller.SecondPersonCamera, controller.FirstPersonPivot, true);
        }
    
        public void SetTarget(SidePersonCamera camera, UnityEngine.Transform followTransform)
        {
            camera.SetTarget(followTransform);
            camera.OrbitCamera.SetFollowTransform(followTransform);
            SetFallowRotation(camera, false);
        }

        public void SetCameraActive(SidePersonCamera camera, UnityEngine.Transform firstPersonCameraTransform, bool flag)
        {
            camera.SetCameraActive(flag);

            if (camera.IsActive)
            {
                camera.OrbitCamera.SetInitialPosition(firstPersonCameraTransform.position, firstPersonCameraTransform.rotation);
            }
            else
            {
                SetFallowRotation(camera, false);
            }
        }

        public void SetFallowRotation(SidePersonCamera camera, bool flag = true)
        {
            camera.OrbitCamera.IsFallowRotation = flag;
        }

        public void SetZoomAbility(SidePersonCamera camera, bool isZoomAvaliable, bool isLunched = false)
        {
            float maxZoom = isZoomAvaliable ? 10f : 0f;

            camera.OrbitCamera.MaxDistance = maxZoom;

            if (isLunched)
            {
                camera.OrbitCamera.SetCurrentDistance(maxZoom / 2);
            }
        }
    }
}