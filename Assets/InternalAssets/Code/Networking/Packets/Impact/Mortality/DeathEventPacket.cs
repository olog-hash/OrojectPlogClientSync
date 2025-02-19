using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.Mortality.Components;
using ProjectOlog.Code.Networking.Packets.Mortality.Impact;

namespace ProjectOlog.Code.Networking.Packets.Mortality
{
    public class DeathEventPacket : INetPackageSerializable
    {
        // Impact
        public ImpactEventData ImpactEventData;
        
        // Components
        public DeathData[] DeathDatas;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(
                ImpactEventData,
                DeathDatas);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            ImpactEventData = dataPackage.GetHeadlessCustom<ImpactEventData>();
            DeathDatas = dataPackage.GetHeadlessCustomArray<DeathData>();
        }
    }
}