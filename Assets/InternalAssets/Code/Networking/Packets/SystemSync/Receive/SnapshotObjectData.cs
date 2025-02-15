using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.SystemSync.Receive
{
    public class SnapshotObjectData  : IHeadlessPackageSerializable
    {
        public int ServerID;
        public byte[] TransformData;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(ServerID, TransformData);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            ServerID = dataPackage.GetInt();
            TransformData = dataPackage.GetByteArray();
        }
    }
}