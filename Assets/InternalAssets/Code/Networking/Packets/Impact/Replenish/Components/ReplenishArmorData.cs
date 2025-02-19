using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.Mortality;

namespace ProjectOlog.Code.Networking.Packets.Impact.Replenish.Components
{
    public class ReplenishArmorData : BaseEventData, IHeadlessPackageSerializable
    {
        public int ReplenishArmorCount;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, ReplenishArmorCount);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            EventID = dataPackage.GetUShort();
            ReplenishArmorCount = dataPackage.GetInt();
        }
    }
}