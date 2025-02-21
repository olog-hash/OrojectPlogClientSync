using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.Mortality;

namespace ProjectOlog.Code.Networking.Packets.Impact.Replenish.Components
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