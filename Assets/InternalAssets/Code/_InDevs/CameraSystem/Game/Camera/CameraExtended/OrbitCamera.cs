using System.Collections.Generic;
using ProjectOlog.Code.Game.Characters.KinematicCharacter.Utilits;
using ProjectOlog.Code.Input.Controls;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.CameraSystem.Game.Camera.CameraExtended
{
    
    public class OrbitCamera : MonoBehaviour
    {
        [Header("Framing")]
        public Transform FollowTransform;
        public UnityEngine.Camera Camera;
        public Vector2 FollowPointFraming = new Vector2(0f, 0f);
        public float FollowingSharpness = 10000f;

        [Header("Distance")]
        public float DefaultDistance = 6f;
        public float MinDistance = 0f;
        public float MaxDistance = 10f;
        public float DistanceMovementSpeed = 5f;
        public float DistanceMovementSharpness = 10f;

        [Header("Rotation")]
        public bool InvertX = false;
        public bool InvertY = false;
        public bool BlockVertical = false;
        public bool BlockHorizontal = false;
        public bool IsFallowRotation = false;
        [Range(-90f, 90f)]
        public float DefaultVerticalAngle = 20f;
        [Range(-90f, 90f)]
        public float MinVerticalAngle = -90f;
        [Range(-90f, 90f)]
        public float MaxVerticalAngle = 90f;
        public float RotationSpeed = 1f;
        public float RotationSharpness = 10000f;
        public bool RotateWithPhysicsMover = false;

        [Header("Obstruction")]
        public float ObstructionCheckRadius = 0.2f;
        public LayerMask ObstructionLayers = -1;
        public float ObstructionSharpness = 10000f;
        public List<Collider> IgnoredColliders = new List<Collider>();

        public Transform Transform { get; private set; }

        public Vector3 PlanarDirection { get; set; } //
        public float TargetDistance { get; set; }

        private bool _distanceIsObstructed;
        private float _currentDistance;
        private float _targetVerticalAngle; //
        private float _targetHorizontalAngle;
        private RaycastHit _obstructionHit;
        private int _obstructionCount;
        private RaycastHit[] _obstructions = new RaycastHit[MaxObstructions];
        private float _obstructionTime;
        private Vector3 _currentFollowPosition;
        private Vector3 PlanarForward = new Vector3(0, 0, 1);
        // Добавляем переменную для хранения последней известной позиции
        private Vector3 _lastKnownPosition;
        // Добавляем переменную для хранения последней известной ротации
        private Quaternion _lastKnownRotation;

        private const int MaxObstructions = 32;

        void OnValidate()
        {
            DefaultDistance = Mathf.Clamp(DefaultDistance, MinDistance, MaxDistance);
            DefaultVerticalAngle = Mathf.Clamp(DefaultVerticalAngle, MinVerticalAngle, MaxVerticalAngle);
        }

        void Awake()
        {
            Transform = this.transform;
            PlanarForward = Transform.forward;

            _currentDistance = DefaultDistance;
            TargetDistance = _currentDistance;

            _targetVerticalAngle = 0f;

            PlanarDirection = Vector3.forward;
            
            // Инициализируем последнюю известную позицию текущей позицией камеры
            _lastKnownPosition = Transform.position;
            _lastKnownRotation = Transform.rotation;

            if (FollowTransform != null)
            {
                SetFollowTransform(FollowTransform);
            }
        }

        public void SetFollowTransform(Transform t)
        {
            FollowTransform = t;
            PlanarDirection = FollowTransform.forward;
            _currentFollowPosition = FollowTransform.position;
            // Сохраняем начальную позицию и ротацию
            _lastKnownPosition = FollowTransform.position;
            _lastKnownRotation = FollowTransform.rotation;

            Transform.rotation = Quaternion.LookRotation(FollowTransform.forward, FollowTransform.up);
        }

        public void ResetViewPositon()
        {
            if (FollowTransform != null)
            {
                PlanarDirection = FollowTransform.forward;
                _currentFollowPosition = FollowTransform.position;
                _lastKnownPosition = FollowTransform.position;
                _lastKnownRotation = FollowTransform.rotation;

                Transform.rotation = Quaternion.LookRotation(FollowTransform.forward, FollowTransform.up);
            }
        }   
        
        public void SyncWithTargetRotation()
        {
            if (FollowTransform != null)
            {
                // Получаем текущую вертикальную составляющую поворота (наклон)
                Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);
        
                // Применяем ротацию объекта слежения
                Transform.rotation = FollowTransform.rotation * verticalRot;
        
                // Получаем углы из итоговой ротации
                Vector3 newAngles = Transform.rotation.eulerAngles;
        
                // Обновляем целевые углы для дальнейшего свободного вращения
                _targetHorizontalAngle = newAngles.y;
                _targetVerticalAngle = newAngles.x > 180 ? newAngles.x - 360 : newAngles.x;
            }
        }
        
        public void SetInitialPosition(Vector3 initialPosition, Quaternion initialRotation)
        {
            _currentFollowPosition = initialPosition;
            _lastKnownPosition = initialPosition;
            _lastKnownRotation = initialRotation;
            Transform.rotation = initialRotation;

            Vector3 initialAngles = initialRotation.eulerAngles;

            float clampedXAngle = initialAngles.x;
            if (clampedXAngle > 180f)
            {
                clampedXAngle -= 360f;
            }

            clampedXAngle = Mathf.Clamp(clampedXAngle, -90f, 90f);

            _targetVerticalAngle = clampedXAngle;
            _targetHorizontalAngle = initialAngles.y;
        }

        public void SetCurrentDistance(float currentDistance)
        {
            DefaultDistance = currentDistance;
            _currentDistance = DefaultDistance;
            TargetDistance = _currentDistance;
        }

        public void Update()
        {
            Vector2 lookInput = InputControls.GetLookAxis();

            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInput = Vector2.zero;
            }

            float scrollInput = -InputControls.GetAxis("Mouse ScrollWheel");

            UpdateWithInput(Time.deltaTime, scrollInput, lookInput);

            if (InputControls.GetMouseButtonDown(1))
            {
                // TargetDistance = (TargetDistance == 0f) ? DefaultDistance : 0f;
            }
        }

        public void UpdateWithInput(float deltaTime, float zoomInput, Vector3 rotationInput)
        {
            // Если FollowTransform существует, обновляем последнюю известную позицию
            if (FollowTransform != null)
            {
                _lastKnownPosition = FollowTransform.position;
                _lastKnownRotation = FollowTransform.rotation;
            }
            // В любом случае используем _lastKnownPosition как точку слежения
            {
                if (InvertX)
                {
                    rotationInput.x *= -1f;
                }
                if (InvertY)
                {
                    rotationInput.y *= -1f;
                }

                Quaternion selfRotation = Transform.rotation;

                // Используем либо FollowTransform, либо последнюю известную позицию
                Transform targetEntity = FollowTransform;
                float rotationSpeed = 2.5f;
                
                // Rotation
                {
                    _targetHorizontalAngle += (rotationInput.x * rotationSpeed);
                    Quaternion horizontalRot = Quaternion.Euler(0, _targetHorizontalAngle, 0);

                    _targetVerticalAngle -= (rotationInput.y * rotationSpeed);
                    _targetVerticalAngle = Mathf.Clamp(_targetVerticalAngle, MinVerticalAngle, MaxVerticalAngle);
                    Quaternion verticalRot = Quaternion.Euler(_targetVerticalAngle, 0, 0);

                    selfRotation = horizontalRot * verticalRot;
                }

                if (IsFallowRotation && FollowTransform != null)
                {
                    selfRotation = FollowTransform.rotation * selfRotation;
                }
                else if (IsFallowRotation)
                {
                    selfRotation = _lastKnownRotation * selfRotation;
                }

                Vector3 cameraForward = MathUtilities.GetForwardFromRotation(selfRotation);

                float desiredDistanceMovementFromInput = zoomInput * DistanceMovementSpeed * deltaTime;
                TargetDistance = Mathf.Clamp(TargetDistance + desiredDistanceMovementFromInput, MinDistance, MaxDistance);
                _currentDistance = Mathf.Lerp(_currentDistance, TargetDistance, MathUtilities.GetSharpnessInterpolant(DistanceMovementSharpness, deltaTime));

                Transform.rotation = selfRotation;

                // Process distance input
                if (_distanceIsObstructed && Mathf.Abs(zoomInput) > 0f)
                {
                    TargetDistance = _currentDistance;
                }
                TargetDistance += zoomInput * DistanceMovementSpeed;
                TargetDistance = Mathf.Clamp(TargetDistance, MinDistance, MaxDistance);

                // Find the smoothed follow position using either current or last known position
                Vector3 targetFollowPosition = FollowTransform != null ? FollowTransform.position : _lastKnownPosition;
                _currentFollowPosition = Vector3.Lerp(_currentFollowPosition, targetFollowPosition, 1f - Mathf.Exp(-FollowingSharpness * deltaTime));

                // Handle obstructions
                {
                    RaycastHit closestHit = new RaycastHit();
                    closestHit.distance = Mathf.Infinity;
                    _obstructionCount = UnityEngine.Physics.SphereCastNonAlloc(_currentFollowPosition, ObstructionCheckRadius, -Transform.forward, _obstructions, TargetDistance, ObstructionLayers, QueryTriggerInteraction.Ignore);
                    for (int i = 0; i < _obstructionCount; i++)
                    {
                        bool isIgnored = false;
                        for (int j = 0; j < IgnoredColliders.Count; j++)
                        {
                            if (IgnoredColliders[j] == _obstructions[i].collider)
                            {
                                isIgnored = true;
                                break;
                            }
                        }

                        if (!isIgnored && _obstructions[i].distance < closestHit.distance && _obstructions[i].distance > 0)
                        {
                            closestHit = _obstructions[i];
                        }
                    }

                    if (closestHit.distance < Mathf.Infinity)
                    {
                        _distanceIsObstructed = true;
                        _currentDistance = Mathf.Lerp(_currentDistance, closestHit.distance, 1 - Mathf.Exp(-ObstructionSharpness * deltaTime));
                    }
                    else
                    {
                        _distanceIsObstructed = false;
                        _currentDistance = Mathf.Lerp(_currentDistance, TargetDistance, 1 - Mathf.Exp(-DistanceMovementSharpness * deltaTime));
                    }
                }

                Vector3 targetPosition = _currentFollowPosition - ((selfRotation * Vector3.forward) * _currentDistance);

                targetPosition += Transform.right * FollowPointFraming.x;
                targetPosition += Transform.up * FollowPointFraming.y;

                Transform.position = targetPosition;
            }
        }
    }
}
