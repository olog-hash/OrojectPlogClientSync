using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Impact.Aggressors;

namespace ProjectOlog.Code.Networking.Packets.Mortality.Impact
{
    public class EnvironmentAggressionData : BaseSubPacketData
    {
        public EEnvironmentType EnvironmentType;
        
        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, (byte)EnvironmentType);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            EnvironmentType = (EEnvironmentType)dataPackage.GetByte();
        }
    }
}