using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Core;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Mortality.Components;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Impact.Mortality
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