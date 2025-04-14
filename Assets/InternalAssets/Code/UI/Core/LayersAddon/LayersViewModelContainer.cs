using System.Collections.Generic;
using ProjectOlog.Code.Infrastructure.Application.Layers;

namespace ProjectOlog.Code.UI.Core
{
    public class LayersViewModelContainer
    {
        private List<ILayer> _layers = new List<ILayer>();
        
        private LayersManager _layersManager;

        public LayersViewModelContainer(LayersManager layersManager)
        {
            _layersManager = layersManager;
        }

        public void RegisterLayer(ILayer layer, ELayerChannel channel, LayerInputMode inputMode)
        {
            _layersManager.RegisterLayer(channel, layer, inputMode);
            _layers.Add(layer);
        }

        public void ClearLayers()
        {
            foreach (var layer in _layers)
            {
                _layersManager.RemoveLayer(layer);
            }
            
            _layers.Clear();
        }
    }
}