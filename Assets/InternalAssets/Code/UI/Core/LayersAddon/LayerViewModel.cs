using ProjectOlog.Code.Infrastructure.Application.Layers;
using Zenject;

namespace ProjectOlog.Code.UI.Core
{
    /// <summary>
    /// Расширенный базовый класс для ViewModel с интеграцией с системой слоев
    /// </summary>
    public abstract class LayerViewModel : BaseViewModel, ILayer
    {
        public string LayerName => LayerInfo?.LayerName;
        public LayerInfo LayerInfo { get; set; }

        protected LayersManager _layersManager;

        [Inject]
        public void Construct(LayersManager layersManager)
        {
            _layersManager = layersManager;
        }
        
        public virtual void OnShowLayer()
        {
            Show();
        }
    
        public virtual void OnHideLayer()
        {
            Hide();
        }

        /// <summary>
        /// Вызывает закрытие самого себя через LayerManager
        /// </summary>
        protected void HideLayerItself()
        {
            if (_layersManager.IsLayerActive(LayerInfo.LayerName))
            {
                _layersManager.HideLayer(LayerInfo.LayerName);
            }
        }
    }
}