using UnityEngine;

namespace ProjectOlog.Code.Game.DiegeticInterface.LookPanel
{
    /// <summary>
    /// Компонент, поворачивающий объект так, чтобы он всегда был обращен к камере
    /// </summary>
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField]
        protected Transform _targetTransform;
        
        public Transform TargetTransform
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
