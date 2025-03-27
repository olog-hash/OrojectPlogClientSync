﻿namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    /// <summary>
    /// Класс для записи регистрации слоя со всем базовыми данными
    /// </summary>
    public class LayerRegistration
    {
        public int LayerID;
        public string Layername;
        public ILayer LayerHandler;
        public LayerInfo LayerInfo;
        
        public bool IsEnabled;

        public LayerRegistration(int layerID, string layername, ILayer layerHandler, LayerInfo layerInfo)
        {
            LayerID = layerID;
            Layername = layername;
            LayerHandler = layerHandler;
            LayerInfo = layerInfo;
            
            IsEnabled = false;
        }

        public void Show()
        {
            IsEnabled = true;
            LayerHandler?.ShowLayer();
        }

        public void Hide()
        {
            IsEnabled = false;
            LayerHandler?.HideLayer();
        }
    }
}