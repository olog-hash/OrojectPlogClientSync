using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets
{
    public class InitUserPacket : INetPackageSerializable
    {
        public int UserID;
        public string Username;
        public bool IsDead;
        public int DeathCount;

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(UserID, Username, IsDead, DeathCount);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            UserID = dataPackage.GetInt();
            Username = dataPackage.GetString();
            IsDead = dataPackage.GetBool();
            DeathCount = dataPackage.GetInt();
        }
    }
}