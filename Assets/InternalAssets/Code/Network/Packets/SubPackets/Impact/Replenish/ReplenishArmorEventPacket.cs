using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Core;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Replenish.Components;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Impact.Replenish
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