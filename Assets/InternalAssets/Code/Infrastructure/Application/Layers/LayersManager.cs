using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    /// <summary>
    /// Центральное управление всеми слоями приложения.
    /// Слои - это элементы игры (преимущественно интерфейсы), которые могут по-разному влиять на игру.
    /// Например, останавливать движение, разблокировать курсор мыши и т.д.
    /// </summary>
    public class LayersManager
    {
        private readonly Dictionary<string, LayerEntry> _layers = new Dictionary<string, LayerEntry>();

        private readonly ApplicationLayersController _applicationLayersController;
        
        public LayersManager(ApplicationLayersController applicationLayersController)
        {
            _applicationLayersController = applicationLayersController;
            
            RegisterPredefinedLayers();
        }
        
        public void Reset()
        {
            _layers.Clear();
            
            RegisterPredefinedLayers();
        }
        
        #region Регистрация слоев
        
        /// <summary>
        /// Регистрирует предопределенные слои, необходимые для базового функционирования
        /// </summary>
        private void RegisterPredefinedLayers()
        {
            RegisterLayer(ELayerChannel.BaseChannel, "MainMenu", LayerInputMode.Freedom);
            RegisterLayer(ELayerChannel.BaseChannel, "Gameplay", LayerInputMode.Game);
        }
        
        /// <summary>
        /// Регистрирует новый слой с указанными параметрами
        /// </summary>
        public bool RegisterLayer(ELayerChannel layerChannel, string layerName, ILayer layer, LayerInputMode layerInputMode)
        {
            if (string.IsNullOrEmpty(layerName))
            {
                return false;
            }
            
            if (!_layers.ContainsKey(layerName))
            {
                
                var layerInfo = new LayerInfo
                {
                    LayerChannel = layerChannel,
                    LayerName = layerName,
                    InputMode = layerInputMode
                };
                
                _layers.Add(layerName, new LayerEntry(layerInfo, layer));
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Регистрирует новый слой, используя имя типа обработчика в качестве имени слоя
        /// </summary>
        public bool RegisterLayer(ELayerChannel layerChannel, ILayer layer, LayerInputMode layerInputMode)
        {
            if (layer == null)
            {
                return false;
            }
            
            var layerName = layer.GetType().Name;
            return RegisterLayer(layerChannel, layerName, layer, layerInputMode);
        }
        
        /// <summary>
        /// Регистрирует новый слой без обработчика
        /// </summary>
        public bool RegisterLayer(ELayerChannel layerChannel, string layerName, LayerInputMode layerInputMode)
        {
            return RegisterLayer(layerChannel, layerName, null, layerInputMode);
        }
        
        #endregion
        
        #region Удаление слоев
        
        /// <summary>
        /// Удаляет слой по имени
        /// </summary>
        public bool RemoveLayer(string layerName)
        {
            if (string.IsNullOrEmpty(layerName))
            {
                return false;
            }
            
            return _layers.Remove(layerName);
        }
        
        /// <summary>
        /// Удаляет слой, используя имя типа обработчика
        /// </summary>
        public bool RemoveLayerByType(ILayer layer)
        {
            if (layer == null)
            {
                return false;
            }
            
            var layerName = layer.GetType().Name;
            return RemoveLayer(layerName);
        }
        
        /// <summary>
        /// Удаляет слой по ссылке на обработчик
        /// </summary>
        public bool RemoveLayer(ILayer layer)
        {
            if (layer == null)
            {
                return false;
            }
            
            var layerEntry = _layers.FirstOrDefault(entry => entry.Value.LayerHandler == layer);
            if (!string.IsNullOrEmpty(layerEntry.Key))
            {
                return _layers.Remove(layerEntry.Key);
            }
            
            return false;
        }

        /// <summary>
        /// Удаляет все слои указанного канала
        /// </summary>
        public bool RemoveLayerChannel(ELayerChannel layerChannel)
        {
            if (layerChannel == ELayerChannel.BaseChannel)
            {
                return false; // Нельзя удалить базовый канал
            }

            var layersToRemove = _layers
                .Where(layer => layer.Value.LayerInfo.LayerChannel == layerChannel)
                .ToList();

            foreach (var layer in layersToRemove)
            {
                _layers.Remove(layer.Key);
            }

            return layersToRemove.Count > 0;
        }
        
        #endregion
        
        #region Запросы информации
        
        /// <summary>
        /// Проверяет, существует ли слой с указанным именем
        /// </summary>
        public bool IsLayerExist(string layerName)
        {
            return !string.IsNullOrEmpty(layerName) && _layers.ContainsKey(layerName);
        }
        
        /// <summary>
        /// Проверяет, доступен ли канал для отображения новых слоев
        /// </summary>
        public bool IsLayerChannelAvailable(ELayerChannel layerChannel)
        {
            return !_layers.Values.Any(layer => layer.LayerInfo.LayerChannel == layerChannel && layer.IsEnabled);
        }

        /// <summary>
        /// Проверяет, может ли слой быть отображен (имеет ли он более высокий приоритет)
        /// </summary>
        public bool IsLayerCanBeShown(string layerName)
        {
            if (!IsLayerExist(layerName)) 
                return false;
            
            if (_layers.TryGetValue(layerName, out LayerEntry layer))
            {
                var highestLayer = GetHighestEnabledLayer();

                if (highestLayer != null)
                {
                    return highestLayer.LayerInfo.LayerChannel < layer.LayerInfo.LayerChannel;
                }
                
                return true; // Нет активных слоев, значит можно показать
            }

            return false;
        }
        
        /// <summary>
        /// Проверяет, активен ли слой в данный момент
        /// </summary>
        public bool IsLayerActive(string layerName)
        {
            if (!IsLayerExist(layerName))
                return false;
                
            return _layers.TryGetValue(layerName, out LayerEntry layer) && layer.IsEnabled;
        }
        
        private LayerEntry GetHighestEnabledLayer()
        {
            return _layers.Values
                .Where(layer => layer.IsEnabled)
                .OrderByDescending(layer => layer.LayerInfo.LayerChannel)
                .FirstOrDefault();
        }
        
        #endregion
        
        #region Управление видимостью
        
        public void ApplyHighestLayer()
        {
            var highestLayer = GetHighestEnabledLayer();
            
            if (highestLayer != null)
            {
                _applicationLayersController.ChangeState(highestLayer.LayerInfo.InputMode);
            }
        }
        
        /// <summary>
        /// Скрывает все слои в указанном канале
        /// </summary>
        public void HideLayersByChannel(ELayerChannel layerChannel)
        {
            foreach (var layer in _layers.Values.Where(layer => 
                layer.LayerInfo.LayerChannel == layerChannel && layer.IsEnabled))
            {
                layer.Hide();
            }
            
            ApplyHighestLayer();
        }
        
        public bool ShowLayer(string layerName)
        {
            if (!IsLayerExist(layerName))
            {
                return false;
            }
            
            if (_layers.TryGetValue(layerName, out LayerEntry layer))
            {
                // Проверяем, можно ли показать слой в его канале (кроме BASE_GAME_CHANNEL)
                if (!IsLayerChannelAvailable(layer.LayerInfo.LayerChannel) && layer.LayerInfo.LayerChannel > ELayerChannel.BaseChannel)
                {
                    return false;
                }
                
                // Если это базовый слой (меню, геймплей), скрываем другие базовые слои
                if (layer.LayerInfo.LayerChannel == ELayerChannel.BaseChannel)
                {
                    HideLayersByChannel(ELayerChannel.BaseChannel);
                }
                
                layer.Show();
                ApplyHighestLayer();
                return true;
            }

            return false;
        }
        
        public bool HideLayer(string layerName)
        {
            if (!IsLayerExist(layerName))
            {
                return false;
            }
            
            if (_layers.TryGetValue(layerName, out LayerEntry layer))
            {
                if (!layer.IsEnabled)
                {
                    return false; // Слой уже скрыт
                }
                
                layer.Hide();
                ApplyHighestLayer();
                return true;
            }

            return false;
        }
        
        #endregion
    }
}