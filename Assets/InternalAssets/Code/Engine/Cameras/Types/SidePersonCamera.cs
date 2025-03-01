using ProjectOlog.Code.Engine.Cameras.Core;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Cameras.Types
{
    /// <summary>
    /// Базовый класс для камеры с боковым видом персонажа.
    /// Управляет орбитальной камерой и виртуальной камерой,
    /// позволяя переключаться между ними и следить за целью.
    /// </summary>
    public class SidePersonCamera : MonoBehaviour
    {
        public UnityEngine.Transform Transform => _transform;
        public bool IsActive => _isActive;
        public UnityEngine.Transform TargetPosition => _followTransform;
        public OrbitCamera OrbitCamera => _orbitCamera;
        public VirtualCamera VirtualCamera => _virtualCamera;
        
        [SerializeField] private OrbitCamera _orbitCamera;
        [SerializeField] private VirtualCamera _virtualCamera;

        private UnityEngine.Transform _transform;
        private UnityEngine.Transform _followTransform;
        private bool _isActive = false;

        public void Initialize()
        {
            _isActive = false;
        }

        public void SetTarget(UnityEngine.Transform followTransform)
        {
            _followTransform = followTransform;
        }

        public void SetCameraActive(bool flag)
        {
            _isActive = flag;
            _orbitCamera.enabled = _isActive;

            if (IsActive)
            {
                _virtualCamera?.RegisterThisCamera();
            }
        }
    }
}