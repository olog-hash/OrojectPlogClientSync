namespace ProjectOlog.Code.Infrastructure.Application.Layers
{
    public class LayerViewInfo
    {
        public int LayerID;
        public string LayerName;
        
        public LayerViewInfo(int layerID, string layerName)
        {
            LayerID = layerID;
            LayerName = layerName;
        }
    }
}