namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    /// <summary>
    /// Класс для записи регистрации слоя со всем базовыми данными
    /// </summary>
    public class LayerEntry
    {
        public LayerInfo LayerInfo;
        public ILayer LayerHandler;
        public bool IsEnabled;

        public LayerEntry(LayerInfo layerInfo, ILayer layerHandler)
        {
            LayerInfo = layerInfo;
            LayerHandler = layerHandler;

            if (layerHandler != null)
            {
                layerHandler.LayerInfo = layerInfo;
            }
            
            IsEnabled = false;
        }

        public void Show()
        {
            IsEnabled = true;
            LayerHandler?.OnShowLayer();
        }

        public void Hide()
        {
            IsEnabled = false;
            LayerHandler?.OnHideLayer();
        }
    }
}