using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Impact.Core.Components;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Impact.Core
{
    public class ImpactEventData : IHeadlessPackageSerializable
    {
        // Impact
        public EnvironmentAggressionData[] EnvironmentAggressionDatas;
        public EntityAggressorData[] EntityAggressorDatas;
        public EntityVictimData[] EntityVictimDatas;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EnvironmentAggressionDatas, EntityAggressorDatas, EntityVictimDatas);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            EnvironmentAggressionDatas = dataPackage.GetHeadlessCustomArray<EnvironmentAggressionData>();
            EntityAggressorDatas = dataPackage.GetHeadlessCustomArray<EntityAggressorData>();
            EntityVictimDatas = dataPackage.GetHeadlessCustomArray<EntityVictimData>();
        }
    }
}