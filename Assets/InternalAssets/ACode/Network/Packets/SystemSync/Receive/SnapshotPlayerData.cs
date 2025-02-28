using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.SystemSync.Receive
{
    public class SnapshotPlayerData  : IHeadlessPackageSerializable
    {
        public byte UserID;
        public byte[] TransformData;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(UserID, TransformData);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            UserID = dataPackage.GetByte();
            TransformData = dataPackage.GetByteArray();
        }
    }
}