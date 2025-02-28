using LiteNetLib;

namespace ProjectOlog.Code.Networking.Infrastructure.Core.Batch
{
    /// <summary>
    /// Содержит мета-данные о отправляемом пакете (который на пути к отправке)
    /// </summary>
    [System.Serializable]
    public struct PackageHeader
    {
        public string NetWorkerName;
        public string MethodName;
        
        public DeliveryMethod DeliveryMethod;
    }
}