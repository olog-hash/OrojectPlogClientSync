using LiteNetLib.Utils;

namespace ProjectOlog.Code.Network.Packets.SubPackets.Users.Components
{
    public class UserIdentityData : IHeadlessPackageSerializable
    {
        public byte UserID;
        public string Username;
        public bool IsAdmin;
        
        public HeadLessDataPacket GetPackage()
        {
            return new HeadLessDataPacket(UserID, Username, IsAdmin);
        }

        public void Deserialize(HeadLessDataPacket dataPackage)
        {
            UserID = dataPackage.GetByte();
            Username = dataPackage.GetString();
            IsAdmin = dataPackage.GetBool();
        }
    }
}