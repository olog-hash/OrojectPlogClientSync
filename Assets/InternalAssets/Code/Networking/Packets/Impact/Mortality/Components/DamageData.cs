using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.Mortality.Components
{
    public class DamageData : BaseEventData, IHeadlessPackageSerializable
    {
        public int DamageCount;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, DamageCount);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            EventID = dataPackage.GetUShort();
            DamageCount = dataPackage.GetInt();
        }
    }
}