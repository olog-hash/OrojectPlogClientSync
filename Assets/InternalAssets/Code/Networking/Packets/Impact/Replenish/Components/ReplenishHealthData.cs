using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.Mortality;

namespace ProjectOlog.Code.Networking.Packets.Impact.Replenish.Components
{
    public class ReplenishHealthData : BaseEventData, IHeadlessPackageSerializable
    {
        public int ReplenishHealthCount;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, ReplenishHealthCount);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            EventID = dataPackage.GetUShort();
            ReplenishHealthCount = dataPackage.GetInt();
        }
    }
}