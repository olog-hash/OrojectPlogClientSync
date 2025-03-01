using ProjectOlog.Code.Engine.Cameras.Core;
using ProjectOlog.Code.Engine.Cameras.Types;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Cameras.ViewControllers
{
    /// <summary>
    /// Содержит ссылки на все переключаемые объекты и камеры, обеспечивая корректное переключение между ними.
    /// </summary>
    public class ChangePersonViewController : ObjectViewController
    {
        public UnityEngine.Transform FirstPersonPivot => _firstPersonPivot;
        public UnityEngine.Transform SecondPersonPivot => _secondPersonPivot;
        public UnityEngine.Transform ThirdPersonPivot => _thirdPersonPivot;
        public VirtualCamera FirstPersonCamera => _firstPersonCamera;
        public SidePersonCamera SecondPersonCamera => _secondPersonCamera;
        
        [Header("Cameras")]
        [SerializeField] private VirtualCamera _firstPersonCamera;
        [SerializeField] private SidePersonCamera _secondPersonCamera;
        
        [Header("Properties")]
        [SerializeField] private UnityEngine.Transform _firstPersonPivot;
        [SerializeField] private UnityEngine.Transform _secondPersonPivot;
        [SerializeField] private UnityEngine.Transform _thirdPersonPivot;
    }
}