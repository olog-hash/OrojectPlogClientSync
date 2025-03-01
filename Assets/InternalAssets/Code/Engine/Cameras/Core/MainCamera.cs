using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ProjectOlog.Code.Engine.Cameras.Core
{
    /// <summary>
    /// Главная камера игры - самый основной скрипт системы камер, буквально глаз. Представляет собой синглтон, который следит за активной виртуальной камерой 
    /// и копирует её позицию и поворот. Также управляет отображением UI и предметных камер через систему стека URP.
    /// </summary>
    public class MainCamera : MonoBehaviour
    {
        public static MainCamera Instance;
        
        [SerializeField] private UnityEngine.Camera _mainCamera;
        [SerializeField] private UnityEngine.Camera _itemsCamera;
        [SerializeField] private UnityEngine.Camera _hudCamera;
        [SerializeField] private UnityEngine.Camera _hudCrosshairCamera;
        
        [Header("Transform")]
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private Quaternion _targetRotation;
        
        private GameObject _gameObject;
        private UnityEngine.Transform _transform;
        private VirtualCamera _currentVirtualCamera;
        
        public void Initialize()
        {
            Instance = this;
            
            _transform = transform;
            _gameObject = gameObject;
            
            _targetPosition = Vector3.zero;
            _targetRotation = Quaternion.identity;
        }
        
        public void InitializeCameras(UnityEngine.Camera hudMain, UnityEngine.Camera hudCrosshair)
        {
            _hudCamera = hudMain;
            _hudCrosshairCamera = hudCrosshair;
            
            var cameraData = _mainCamera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(_hudCrosshairCamera);
            cameraData.cameraStack.Add(_itemsCamera);
            cameraData.cameraStack.Add(_hudCamera);
        }

        private void LateUpdate()
        {
            if (_currentVirtualCamera != null)
            {
                _targetPosition = _currentVirtualCamera.CameraTransform.position;
                _targetRotation = _currentVirtualCamera.CameraTransform.rotation;
            }

            _transform.position = _targetPosition;
            _transform.rotation = _targetRotation;
        }

        public void RegisterVirtualCamera(VirtualCamera virtualCamera)
        {
            _currentVirtualCamera = virtualCamera;
            
            // Обновить параметры
            _mainCamera.fieldOfView = _currentVirtualCamera.POV;
            _itemsCamera.fieldOfView = _currentVirtualCamera.POV;
        }
        
        public void ShowHudCamera(bool flag = true)
        {
            _hudCamera.enabled = flag;
            _hudCrosshairCamera.enabled = flag;
        }

        public void ShowItemsCamera(bool flag = true)
        {
            _itemsCamera.enabled = flag;
        }

        public void ShowMainCamera(bool flag = true)
        {
            _mainCamera.enabled = flag;
        }
    }
}
