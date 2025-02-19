using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.Impact.Replenish.Components;
using ProjectOlog.Code.Networking.Packets.Mortality.Impact;

namespace ProjectOlog.Code.Networking.Packets.Impact.Replenish
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