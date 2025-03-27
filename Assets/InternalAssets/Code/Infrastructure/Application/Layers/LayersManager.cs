using System.Collections.Generic;
using System.Linq;

namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    /// <summary>
    /// Центральное управление всеми слоями приложение.
    /// Слои - это элементы игры (за частую интерфейса) и каждый интерфейс (или его отсутствия) может по разному влиять на игру.
    /// Например останавливать движение, разблокировать курсор мыши и т.д.
    /// </summary>
    public static class LayersManager
    {
        /*
         * Хотелось бы зарегистрировать полностью все окна (с возможностью их удаления после выхода из игры)
         * Эти окна по умолчанию будут отключены, но их можно будет включаить и возможно связывать с обьектами из игры
         */
        
        private static Dictionary<string, LayerRegistration> _layers = new Dictionary<string, LayerRegistration>();

        public static void Reset()
        {
            _layers.Clear();
            
            RegisterPredefinedLayers();
        }
        
        private static void RegisterPredefinedLayers()
        {
            RegisterLayer(0, "MainMenu", null, LayerInfo.Freedom);
            RegisterLayer(0, "Gameplay", null, LayerInfo.Game);
        }
        
        public static bool RegisterLayer(int layerID, string layerName, ILayer layer, LayerInfo layerInfo)
        {
            if (!_layers.ContainsKey(layerName))
            {
                _layers.Add(layerName, new LayerRegistration(layerID, layerName, layer, layerInfo));
                return true;
            }
            return false;
        }

        public static bool RemoveLayer(string layerName)
        {
            return _layers.Remove(layerName);
        }

        public static bool RemoveLayerChannel(int layerId)
        {
            if (layerId == 0)
            {
                return false;
            }

            var layersToRemove = _layers.Where(layer => layer.Value.LayerID == layerId).ToList();

            foreach (var layer in layersToRemove)
            {
                _layers.Remove(layer.Key);
            }

            return layersToRemove.Count > 0;
        }

        public static bool IsLayerExist(string layerName)
        {
            return _layers.ContainsKey(layerName);
        }
        
        public static bool IsLayerChannelAvailable(int layerId)
        {
            return !_layers.Values.Any(layer => layer.LayerID == layerId && layer.IsEnabled);
        }

        public static bool IsLayerCanBeShown(string layerName)
        {
            if (!IsLayerExist(layerName)) return false;
            
            if (_layers.TryGetValue(layerName, out LayerRegistration layer))
            {
                var highestLayer = _layers.Values.Where(layer => layer.IsEnabled)
                    .OrderByDescending(layer => layer.LayerID)
                    .FirstOrDefault();

                if (highestLayer != null)
                {
                    return highestLayer.LayerID < layer.LayerID;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }
        
        // Скрывает все слои в нужном канале
        public static void HideLayersByChannelId(int layerId)
        {
            foreach (var layer in _layers.Values.Where(layer => layer.LayerID == layerId && layer.IsEnabled))
            {
                layer.Hide();
            }
            
            ApplyHighestLayer();
        }

        // Перебирает все включенные слои и активирует только тот, который имеет наивысший приоритет (самое большое layerID)
        public static void ApplyHighestLayer()
        {
            var highestLayer = _layers.Values.Where(layer => layer.IsEnabled)
                .OrderByDescending(layer => layer.LayerID)
                .FirstOrDefault();
            
            if (highestLayer != null)
            {
                ApplicationLayersController.ChangeState(highestLayer.LayerInfo);
            }
        }

        public static bool IsLayerActive(string layerName)
        {
            if (_layers.TryGetValue(layerName, out LayerRegistration layer))
            {
                return layer.IsEnabled;
            }
            
            return false;
        }

        public static bool ShowLayer(string layerName)
        {
            if (!IsLayerExist(layerName))
            {
                return false;
            }
            
            if (_layers.TryGetValue(layerName, out LayerRegistration layer))
            {
                if (!IsLayerChannelAvailable(layer.LayerID) && layer.LayerID > 0)
                {
                    return false;
                }

                if (layer.LayerID == 0)
                {
                    HideLayersByChannelId(0);
                }
                
                layer.Show();
                ApplyHighestLayer();
            }

            return true;
        }
        
        public static bool HideLayer(string layerName)
        {
            if (!IsLayerExist(layerName))
            {
                return false;
            }
            
            if (_layers.TryGetValue(layerName, out LayerRegistration layer))
            {
                if (!layer.IsEnabled)
                {
                    return false;
                }
                
                layer.Hide();
                ApplyHighestLayer();
            }

            return true;
        }
    }
}