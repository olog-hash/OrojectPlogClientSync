using LiteNetLib.Utils;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Based
{
    /// <summary>
    /// Базовый класс для пакетов (суб-пактов data).
    /// Содержит в себе ushort EntityID для возможности ссылаться на общую сущность для каждого компонента.
    /// </summary>
    public abstract class BaseSubPacketData : IHeadlessPackageSerializable
    {
        /// <summary>
        /// Ссылка на ID той сущности, к которой прикреплен компонент.
        /// СЕРИАЛИЗАЦИЯ ОБЯЗАТЕЛЬНА (если в наследуемом компоненте есть какая-либо переменная).
        /// В ином случаи, сериализации только EntityID прописана в базовом классе.
        /// Убедитесь, что наследники сериализуют EntityID в "GetPacket()", если переопределяют метод.
        /// </summary>
        public ushort EventID { get; set; }
        
        /// Убедитесь, что наследники сериализуют EntityID в "GetPacket()", если переопределяют метод.
        public virtual HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID);
        }

        /// <summary>
        /// Для удобства - EntityID должен ДЕсериализовываться только в одном методе.
        /// Чтобы в будущем, если потребуется, можно было легко поменять тип десериализации.
        /// </summary>
        public virtual void Deserialize(HeadLessDataPacket dataPackage)
        {
            EventID = dataPackage.GetUShort();
        }
    }
}