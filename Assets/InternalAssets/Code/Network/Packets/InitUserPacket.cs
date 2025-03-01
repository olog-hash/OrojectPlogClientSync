using LiteNetLib.Utils;

namespace ProjectOlog.Code.Network.Packets
{
    public class InitUserPacket : INetPackageSerializable
    {
        public byte UserID;
        public string Username;
        public bool IsDead;
        public int DeathCount;

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(UserID, Username, IsDead, DeathCount);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            UserID = dataPackage.GetByte();
            Username = dataPackage.GetString();
            IsDead = dataPackage.GetBool();
            DeathCount = dataPackage.GetInt();
        }
    }
}