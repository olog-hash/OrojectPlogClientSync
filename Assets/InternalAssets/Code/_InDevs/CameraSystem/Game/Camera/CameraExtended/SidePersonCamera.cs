using ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.Core;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.CameraExtended
{
    /// <summary>
    /// 
    /// </summary>
    public class SidePersonCamera : MonoBehaviour
    {
        public Transform Transform => _transform;
        public bool IsActive => _isActive;
        public Transform TargetPosition => _followTransform;
        public OrbitCamera OrbitCamera => _orbitCamera;
        public VirtualCamera VirtualCamera => _virtualCamera;
        
        [SerializeField] private OrbitCamera _orbitCamera;
        [SerializeField] private VirtualCamera _virtualCamera;

        private Transform _transform;
        private Transform _followTransform;
        private bool _isActive = false;

        public void Initialize()
        {
            _isActive = false;
        }

        public void SetTarget(Transform followTransform)
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