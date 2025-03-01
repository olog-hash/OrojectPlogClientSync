using ProjectOlog.Code.Engine.Cameras.ViewControllers;

namespace ProjectOlog.Code.Engine.Cameras.Services
{
    /// <summary>
    /// Сервис для управления камерой в режиме от первого лица.
    /// </summary>
    public class FirstPersonCameraService
    {
        public void SwitchToFirstPerson(ChangePersonViewController controller)
        {
            controller.FirstPersonCamera.RegisterThisCamera();
        }
    }
}