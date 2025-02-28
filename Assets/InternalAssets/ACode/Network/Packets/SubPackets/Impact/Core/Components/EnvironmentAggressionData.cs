using LiteNetLib.Utils;
using ProjectOlog.Code.Mechanics.Impact.Aggressors;
using ProjectOlog.Code.Networking.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Core.Components
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