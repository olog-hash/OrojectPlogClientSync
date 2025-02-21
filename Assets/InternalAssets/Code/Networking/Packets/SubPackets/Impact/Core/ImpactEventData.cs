using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.Mortality.Impact
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