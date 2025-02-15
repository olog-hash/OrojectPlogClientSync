using System.Collections.Generic;
using ProjectOlog.Code._InDevs.CameraSystem.Game.ViewControllers.CameraViews.Markers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.CameraSystem.Game.ViewControllers.CameraViews
{
    /// <summary>
    /// Управляет переключением видимости объектов между режимами просмотра (от первого/третьего лица).
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

                if (component is FirstPersonObjectViewMarker fpvp)
                {
                    _firstPersonComponents.Add(fpvp);
                }
                else if (component is ThirdPersonObjectViewMarker tpvp)
                {
                    _thirdPersonComponents.Add(tpvp);
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

            foreach (var fpvp in _firstPersonComponents)
            {
                fpvp.GlobalEnable = isFirstPerson;
            }

            foreach (var tpvp in _thirdPersonComponents)
            {
                tpvp.GlobalEnable = !isFirstPerson;
            }
        }
        
        public void HideAllViews()
        {
            foreach (var fpvp in _firstPersonComponents)
            {
                fpvp.GlobalEnable = false;
            }

            foreach (var tpvp in _thirdPersonComponents)
            {
                tpvp.GlobalEnable = false;
            }
        }
    }
}