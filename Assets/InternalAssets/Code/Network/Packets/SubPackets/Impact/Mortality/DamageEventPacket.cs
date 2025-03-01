using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Core;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Mortality.Components;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Impact.Mortality
{
    public class DamageEventPacket : INetPackageSerializable
    {
        // Impact
        public ImpactEventData ImpactEventData;
        
        // Components
        public DamageData[] DamageDatas;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(
                ImpactEventData,
                DamageDatas);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            ImpactEventData = dataPackage.GetHeadlessCustom<ImpactEventData>();
            DamageDatas = dataPackage.GetHeadlessCustomArray<DamageData>();
        }
    }
}