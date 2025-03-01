using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Core;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Replenish.Components;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Impact.Replenish
{
    
    public class ReplenishHealthEventPacket : INetPackageSerializable
    {
        // Impact
        public ImpactEventData ImpactEventData;
        
        // Components
        public ReplenishHealthData[] ReplenishHealthDatas;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(
                ImpactEventData,
                ReplenishHealthDatas);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            ImpactEventData = dataPackage.GetHeadlessCustom<ImpactEventData>();
            ReplenishHealthDatas = dataPackage.GetHeadlessCustomArray<ReplenishHealthData>();
        }
    }
}