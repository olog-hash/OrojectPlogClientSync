using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.SystemSync.Receive
{
    public class SnapshotPlayerData  : IHeadlessPackageSerializable
    {
        public int UserID;
        public byte[] TransformData;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(UserID, TransformData);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            UserID = dataPackage.GetInt();
            TransformData = dataPackage.GetByteArray();
        }
    }
}