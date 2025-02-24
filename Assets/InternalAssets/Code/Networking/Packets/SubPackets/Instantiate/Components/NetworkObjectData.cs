using LiteNetLib.Utils;
using ProjectOlog.Code.Core.Enums;
using ProjectOlog.Code.Networking.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate.Components
{
    public class NetworkObjectData : BaseSubPacketData
    {
        public ENetworkObjectType NetworkObjectType;
        
        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, (byte)NetworkObjectType);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);

            NetworkObjectType = (ENetworkObjectType)dataPackage.GetByte();
        }
    }
}