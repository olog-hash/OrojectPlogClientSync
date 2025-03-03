using LiteNetLib;

namespace ProjectOlog.Code.Network.Infrastructure.Core.Batch
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
        
        /// <summary>
        /// Должны ли однотипные пакеты быть склеены в один.
        /// </summary>
        public bool IsShouldIgnoreBatching;
    }
}