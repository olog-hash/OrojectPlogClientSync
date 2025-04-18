using R3;
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
        [SerializeField] private UnityEngine.Camera _hudCrosshairCamera;
        
        [Header("Transform")]
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private Quaternion _targetRotation;
        
        private UnityEngine.Transform _transform;
        private VirtualCamera _currentVirtualCamera;
        
        // Статический доступ к видимости камеры предметов
        private static ReactiveProperty<bool> _itemsCameraVisibility = new ReactiveProperty<bool>(true);
        public static ReadOnlyReactiveProperty<bool> ItemsCameraVisibility => _itemsCameraVisibility.ToReadOnlyReactiveProperty();

        private CompositeDisposable _disposables = new CompositeDisposable();
        
        public void Initialize()
        {
            Instance = this;
            
            _transform = transform;
            
            _targetPosition = Vector3.zero;
            _targetRotation = Quaternion.identity;
            
            // Подписываемся на изменения статической видимости
            _itemsCameraVisibility
                .Subscribe(visible => {
                    if (Instance != null)
                        Instance.ShowItemsCamera(visible);
                })
                .AddTo(_disposables);
        }
        
        public void InitializeCameras(UnityEngine.Camera hudCrosshair)
        {
            _hudCrosshairCamera = hudCrosshair;
            
            var cameraData = _mainCamera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(_hudCrosshairCamera);
            cameraData.cameraStack.Add(_itemsCamera);
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

        public void ShowItemsCamera(bool flag = true)
        {
            _itemsCamera.enabled = flag;
        }

        public void ShowMainCamera(bool flag = true)
        {
            _mainCamera.enabled = flag;
        }
        
        // Статические методы для управления видимостью камеры предметов
        public static void SetItemsCameraVisibility(bool visible) => _itemsCameraVisibility.Value = visible;
        public static void ToggleItemsCamera() => _itemsCameraVisibility.Value = !_itemsCameraVisibility.Value;
        
        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
                
            _disposables.Dispose();
        }
    }
}
