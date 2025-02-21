using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.Impact.Replenish.Components;
using ProjectOlog.Code.Networking.Packets.Mortality.Impact;

namespace ProjectOlog.Code.Networking.Packets.Impact.Replenish
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