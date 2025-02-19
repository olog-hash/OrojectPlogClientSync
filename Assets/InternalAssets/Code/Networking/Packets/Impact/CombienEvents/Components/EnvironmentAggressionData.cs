using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Impact.Aggressors;

namespace ProjectOlog.Code.Networking.Packets.Mortality.Impact
{
    public class EnvironmentAggressionData : BaseEventData, IHeadlessPackageSerializable
    {
        public EEnvironmentType EnvironmentType;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, (byte)EnvironmentType);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            EventID = dataPackage.GetUShort();
            EnvironmentType = (EEnvironmentType)dataPackage.GetByte();
        }
    }
}