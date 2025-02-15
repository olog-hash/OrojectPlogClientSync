using LiteNetLib.Utils;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Packets
{
    public class InitPlayerPacket : INetPackageSerializable
    {
        public int ServerID;
        public int UserID;
        public Vector3 Position;
        public Quaternion Rotation;
        
        // TODO: Прочая информация о ЖИВОМ игроке
        public bool IsDead;
        public ushort Health;
        public ushort Armor;

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(ServerID, UserID, Position, Rotation, IsDead, Health, Armor);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            ServerID = dataPackage.GetInt();
            UserID = dataPackage.GetInt();
            Position = dataPackage.GetVector3();
            Rotation = dataPackage.GetVector4();
            
            IsDead = dataPackage.GetBool();
            Health = dataPackage.GetUShort();
            Armor = dataPackage.GetUShort();
        }
    }
}