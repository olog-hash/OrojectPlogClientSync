using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Mortality.Components
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