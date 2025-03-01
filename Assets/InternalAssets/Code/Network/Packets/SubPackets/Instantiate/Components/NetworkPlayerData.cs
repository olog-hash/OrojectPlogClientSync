using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Instantiate.Components
{
    public class NetworkPlayerData : BaseSubPacketData
    {
        public byte UserID;
        public ushort LastStateVersion;

        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, UserID, LastStateVersion);
        }
        
        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            UserID = dataPackage.GetByte();
            LastStateVersion = dataPackage.GetUShort();
        }
    }
}