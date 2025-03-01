using System.Collections.Generic;
using ProjectOlog.Code.Engine.Cameras.ViewControllers.Markers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectOlog.Code.Engine.Cameras.ViewControllers
{
    /// <summary>
    /// Управляет переключением видимости объектов между режимами просмотра (от первого/третьего лица).
    /// Находит и контролирует маркеры объектов для обеспечения правильной видимости компонентов.
    /// </summary>
    public class ObjectViewController : MonoBehaviour
    {
        public bool IsFirstPersonView => _isFirstPerson;
        
        // Списки для хранения компонентов видов
        private List<FirstPersonObjectViewMarker> _firstPersonComponents = new List<FirstPersonObjectViewMarker>();
        private List<ThirdPersonObjectViewMarker> _thirdPersonComponents = new List<ThirdPersonObjectViewMarker>();
        
        private bool _isFirstPerson;

        private void Awake()
        {
            FindViewComponents();
        }

        // Поиск и сохранение компонентов видов
        [Button("Инициализировать")]
        private void FindViewComponents()
        {
            _firstPersonComponents.Clear();
            _thirdPersonComponents.Clear();

            BaseObjectViewMarker[] components = GetComponentsInChildren<BaseObjectViewMarker>(true);

            foreach (var component in components)
            {
                component.Initialize();

                if (component is FirstPersonObjectViewMarker firstPersonMarker)
                {
                    _firstPersonComponents.Add(firstPersonMarker);
                }
                else if (component is ThirdPersonObjectViewMarker thirdPersonMarker)
                {
                    _thirdPersonComponents.Add(thirdPersonMarker);
                }
            }
        }
        
        [Button("Вид от первого лица")]
        public void SwitchToFirstPersonView()
        {
            SetViewMode(true);
        }
        
        [Button("Вид от третьего лица")]
        public void SwitchToThirdPersonView()
        {
            SetViewMode(false);
        }
        
        private void SetViewMode(bool isFirstPerson)
        {
            _isFirstPerson = isFirstPerson;

            // Активируем объекты для выбранного режима и деактивируем для другого
            foreach (var firstPersonMarker in _firstPersonComponents)
            {
                firstPersonMarker.GlobalEnable = isFirstPerson;
            }

            foreach (var thirdPersonMarker in _thirdPersonComponents)
            {
                thirdPersonMarker.GlobalEnable = !isFirstPerson;
            }
        }
        
        public void HideAllViews()
        {
            foreach (var firstPersonMarker in _firstPersonComponents)
            {
                firstPersonMarker.GlobalEnable = false;
            }

            foreach (var thirdPersonMarker in _thirdPersonComponents)
            {
                thirdPersonMarker.GlobalEnable = false;
            }
        }
    }
}