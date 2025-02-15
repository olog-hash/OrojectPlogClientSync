using ProjectOlog.Code._InDevs.CameraSystem.Game.ViewControllers.CameraViews;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.CameraExtended.CamerasLogic
{
    public class FirstPersonCameraService
    {
        public void SwitchToFirstPerson(ChangePersonViewController controller)
        {
            controller.FirstPersonCamera.RegisterThisCamera();
        }
    }
}