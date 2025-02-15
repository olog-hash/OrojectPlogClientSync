using UnityEngine;

namespace ProjectOlog.Code.Entities.Objects.Interactables.Core
{
    public class RotateInterpolation
    {
        public Quaternion CurrentRotation { get; private set; }

        private Vector3 _targetRotation;
        private Quaternion _startRotation; 
        private float _duration = 1f;
        private float _elapsedTime = 0f; 
        private bool _isRotating = false; 

        private Transform _targetTransform;
        
        public RotateInterpolation(Transform targetTransform, Vector3 targetRotation, float duration = 1)
        {
            _targetTransform = targetTransform;
            _startRotation = _targetTransform.localRotation;
            _targetRotation = targetRotation;
            CurrentRotation = _startRotation;
            
            _duration = duration;
            _elapsedTime = 0f;
            _isRotating = false;
        }

        public void StartRotation()
        {
            if (!_isRotating)
            {
                _startRotation = _targetTransform.localRotation;
                CurrentRotation = _startRotation;
                _elapsedTime = 0f;
                _isRotating = true;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            if (_isRotating)
            {
                _elapsedTime += deltaTime;
                float t = Mathf.Clamp01(_elapsedTime / _duration);
                CurrentRotation = Quaternion.Slerp(_startRotation, Quaternion.Euler(_targetRotation), t);

                if (t >= 1f)
                {
                    _isRotating = false;
                }
            }
        }
    }
}