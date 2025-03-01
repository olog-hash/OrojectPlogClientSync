using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Impact.Replenish.Components
{
    public class ReplenishArmorData : BaseSubPacketData
    {
        public int ReplenishArmorCount;
        
        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, ReplenishArmorCount);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            ReplenishArmorCount = dataPackage.GetInt();
        }
    }
}