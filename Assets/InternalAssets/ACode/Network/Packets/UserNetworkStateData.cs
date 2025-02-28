using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets
{
    public class UserNetworkStateData : INetPackageSerializable
    {
        public byte UserID;
        public short Ping;
        public bool IsDead;
        public int DeathCount;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(UserID, Ping, IsDead, DeathCount );
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            UserID = dataPackage.GetByte();
            Ping = dataPackage.GetShort();
            IsDead = dataPackage.GetBool();
            DeathCount = dataPackage.GetInt();
        }
    }
}