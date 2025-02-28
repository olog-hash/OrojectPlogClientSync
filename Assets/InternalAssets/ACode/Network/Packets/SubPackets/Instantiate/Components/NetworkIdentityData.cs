using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate.Components
{
    public class NetworkIdentityData : BaseSubPacketData
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