using UnityEngine;

namespace ProjectOlog.Code.Game.DiegeticInterface.LookPanel
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform _panelTransform;

        public void Awake()
        {
            _panelTransform = transform;
        }

        void LateUpdate()
        {
            if (UnityEngine.Camera.main == null || _panelTransform == null) return;

            _panelTransform.LookAt(_panelTransform.position + UnityEngine.Camera.main.transform.forward);
        }
    }
}
