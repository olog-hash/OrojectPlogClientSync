using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Impact.Core.Components
{
    public class EntityVictimData : BaseSubPacketData
    {
        public ushort ServerID;
        
        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, ServerID);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            ServerID = dataPackage.GetUShort();
        }
    }
}