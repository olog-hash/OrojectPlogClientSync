namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
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
            LayerHandler?.OnShow();
        }

        public void Hide()
        {
            IsEnabled = false;
            LayerHandler?.OnHide();
        }
    }
}