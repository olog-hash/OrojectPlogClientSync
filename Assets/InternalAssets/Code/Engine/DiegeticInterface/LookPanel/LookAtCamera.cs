using UnityEngine;

namespace ProjectOlog.Code.Engine.DiegeticInterface.LookPanel
{
    /// <summary>
    /// Компонент, поворачивающий объект так, чтобы он всегда был обращен к камере
    /// </summary>
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField]
        protected UnityEngine.Transform _targetTransform;
        
        public UnityEngine.Transform TargetTransform
        {
            get
            {
                if (_targetTransform == null)
                {
                    _targetTransform = transform;
                }
                return _targetTransform;
            }
        }
        
        private void LateUpdate()
        {
            if (Camera.main == null) 
                return;
            
            Vector3 cameraForward = Camera.main.transform.forward;
        
            TargetTransform.LookAt(TargetTransform.position + cameraForward);
        }
    }
}
