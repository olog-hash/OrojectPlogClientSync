using UnityEngine;

namespace ProjectOlog.Code.Engine.Cameras.ViewControllers.Markers
{
    /// <summary>
    /// Базовый класс для маркеров объектов с различными режимами видимости.
    /// Управляет активностью объекта на основе глобального и локального флагов.
    /// </summary>
    public abstract class BaseObjectViewMarker : MonoBehaviour
    {
        // Флаг глобальной активности (управляется системой переключения камер)
        private bool _globalEnable = true;
        
        // Флаг локальной активности (управляется самим объектом или другими системами)
        private bool _localEnable = true;
        
        /// <summary>
        /// Глобальный флаг активности, контролируемый системой переключения камер.
        /// </summary>
        public bool GlobalEnable
        {
            get => _globalEnable;
            set
            {
                _globalEnable = value;
                UpdateVisibility();
            }
        }
        
        /// <summary>
        /// Локальный флаг активности, который может быть изменен
        /// конкретным объектом независимо от режима камеры.
        /// </summary>
        public bool LocalEnable
        {
            get => _localEnable;
            set
            {
                _localEnable = value;
                UpdateVisibility();
            }
        }
        
        public void Initialize()
        {
            // Настраиваем начальное состояние видимости
            UpdateVisibility();
        }
        
        // Обновляет активность объекта на основе глобального и локального флагов.
        private void UpdateVisibility()
        {
            gameObject.SetActive(GlobalEnable && LocalEnable);
        }
    }
}