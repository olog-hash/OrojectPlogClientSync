using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Replenish.Components
{
    public class ReplenishHealthData : BaseSubPacketData
    {
        public int ReplenishHealthCount;
        
        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, ReplenishHealthCount);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            ReplenishHealthCount = dataPackage.GetInt();
        }
    }
}