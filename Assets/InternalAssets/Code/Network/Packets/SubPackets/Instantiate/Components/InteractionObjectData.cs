using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Instantiate.Components
{
    public class InteractionObjectData : BaseSubPacketData
    {
        public NetDataPackage SnapshotObjectData;

        public override HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(EventID, SnapshotObjectData);
        }

        public override void Deserialize(HeadLessDataPacket dataPackage)
        {
            base.Deserialize(dataPackage);
            
            SnapshotObjectData = dataPackage.GetPackage();
        }
    }
}