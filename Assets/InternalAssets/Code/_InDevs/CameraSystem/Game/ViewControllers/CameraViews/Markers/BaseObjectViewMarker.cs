using UnityEngine;

namespace ProjectOlog.Code._InDevs.CameraSystem.Game.ViewControllers.CameraViews.Markers
{
    public abstract class BaseObjectViewMarker : MonoBehaviour
    {
        private GameObject _thisObject;
        private bool _globalEnable = true;
        private bool _localEnable = true;
        
        [SerializeField]
        public bool GlobalEnable
        {
            get => _globalEnable;
            set
            {
                _globalEnable = value;
                CheckAndSetActive();
            }
        }
        
        [SerializeField]
        public bool LocalEnable
        {
            get => _localEnable;
            set
            {
                _localEnable = value;
                CheckAndSetActive();
            }
        }
        
        public void Initialize()
        {
            _thisObject = this.gameObject;
        }
        
        private void CheckAndSetActive()
        {
            gameObject.SetActive(GlobalEnable && LocalEnable);
        }
    }
}