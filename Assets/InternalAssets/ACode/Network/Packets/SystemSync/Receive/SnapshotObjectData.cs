using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.SystemSync.Receive
{
    public class SnapshotObjectData  : IHeadlessPackageSerializable
    {
        public ushort ServerID;
        public byte[] TransformData;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(ServerID, TransformData);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            ServerID = dataPackage.GetUShort();
            TransformData = dataPackage.GetByteArray();
        }
    }
}