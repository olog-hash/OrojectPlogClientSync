using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Cameras.Core
{
    /// <summary>
    /// Базовый компонент виртуальной камеры, который определяет откуда и как смотреть на игровой мир.
    /// Регистрируется в MainCamera и передает ей свои параметры положения и поворота для отображения.
    /// </summary>
    public class VirtualCamera : MonoBehaviour
    {
        public UnityEngine.Transform CameraTransform => _transform;
        
        // Data
        public float POV = 60;

        // Tools
        private UnityEngine.Transform _transform;

        private void Awake()
        {
            _transform = transform;
        }

        [Button("Register")]
        public void RegisterThisCamera()
        {
            if (MainCamera.Instance != null)
            {
                MainCamera.Instance.RegisterVirtualCamera(this);
            }
            else
            {
                Debug.LogError("There are no main camera!");
            }
        }
    }
}