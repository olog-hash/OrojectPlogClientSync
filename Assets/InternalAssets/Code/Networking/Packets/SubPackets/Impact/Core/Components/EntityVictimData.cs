using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Impact.Core.Components
{
    public class EntityVictimData : BaseSubPacketData
    {
        public int ServerID;
        
        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, ServerID);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            ServerID = dataPackage.GetInt();
        }
    }
}