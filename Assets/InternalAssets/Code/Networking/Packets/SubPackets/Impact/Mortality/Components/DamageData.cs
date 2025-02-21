using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.Mortality.Components
{
    public class DamageData : BaseSubPacketData
    {
        public int DamageCount;
        
        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, DamageCount);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            DamageCount = dataPackage.GetInt();
        }
    }
}