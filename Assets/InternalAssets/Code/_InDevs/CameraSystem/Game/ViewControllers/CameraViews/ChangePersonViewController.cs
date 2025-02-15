using ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.CameraExtended;
using ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.Core;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.CameraSystem.Game.ViewControllers.CameraViews
{
    /// <summary>
    /// Содержит ссылки на все переключаемые объекты и камеры, обеспечивая корректное переключение между ними.
    /// </summary>
    public class ChangePersonViewController : ObjectViewController
    {
        public Transform FirstPersonPivot => _firstPersonPivot;
        public Transform SecondPersonPivot => _secondPersonPivot;
        public Transform ThirdPersonPivot => _thirdPersonPivot;
        public VirtualCamera FirstPersonCamera => _firstPersonCamera;
        public SidePersonCamera SecondPersonCamera => _secondPersonCamera;
        
        [Header("Cameras")]
        [SerializeField] private VirtualCamera _firstPersonCamera;
        [SerializeField] private SidePersonCamera _secondPersonCamera;
        
        [Header("Properties")]
        [SerializeField] private Transform _firstPersonPivot;
        [SerializeField] private Transform _secondPersonPivot;
        [SerializeField] private Transform _thirdPersonPivot;
    }
}