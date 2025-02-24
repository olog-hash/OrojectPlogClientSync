using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Core;
using ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Mortality.Components;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Mortality
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