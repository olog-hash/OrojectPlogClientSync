using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Packets.SubPackets.Based;

namespace ProjectOlog.Code.Networking.Packets.SubPackets.Instantiate.Components
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