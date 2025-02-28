using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Core;
using ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Replenish.Components;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Replenish
{
    public class ReplenishArmorEventPacket : INetPackageSerializable
    {
        // Impact
        public ImpactEventData ImpactEventData;
        
        // Components
        public ReplenishArmorData[] ReplenishArmorDatas;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(
                ImpactEventData,
                ReplenishArmorDatas);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            ImpactEventData = dataPackage.GetHeadlessCustom<ImpactEventData>();
            ReplenishArmorDatas = dataPackage.GetHeadlessCustomArray<ReplenishArmorData>();
        }
    }
}