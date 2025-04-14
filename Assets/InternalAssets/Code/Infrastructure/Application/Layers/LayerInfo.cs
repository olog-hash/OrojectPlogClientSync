namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    public enum ELayerChannel
    {
        /// <summary>
        /// Базовый канал для основных элементов (меню, игровой процесс)
        /// </summary>
        BaseChannel = 0,
        
        // Остальные каналы
        Channel_1,
        Channel_2,
        Channel_3,
        Channel_4,
        Channel_5,
        Channel_6,
        Channel_7,
        Channel_8,
    }
    
    public class LayerInfo
    {
        public ELayerChannel LayerChannel { get; set; }
        public string LayerName { get; set; }
        public LayerInputMode InputMode { get; set; }
    }
}