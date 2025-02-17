using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.Mortality.Components;
using ProjectOlog.Code.Networking.Packets.Mortality.Impact;

namespace ProjectOlog.Code.Networking.Packets.Mortality
{
    public class DamageEventPacket : INetPackageSerializable
    {
        // Impact
        public ImpactEventData ImpactEventData;
        
        // Additional Components
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