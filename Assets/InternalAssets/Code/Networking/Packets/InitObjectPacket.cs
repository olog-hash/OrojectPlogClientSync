using LiteNetLib.Utils;
using ProjectOlog.Code.Core.Enums;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Packets
{
    public class InitObjectPacket : INetPackageSerializable
    {
        public int ServerID;
        public ENetworkObjectType ObjectType;
        public Vector3 Position;
        public Quaternion Rotation;

        // Network object data
        public NetDataPackage ObjectData;

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(ServerID, (byte)ObjectType, Position, Rotation, ObjectData);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            ServerID = dataPackage.GetInt();
            ObjectType = (ENetworkObjectType)dataPackage.GetByte();
            Position = dataPackage.GetVector3();
            Rotation = dataPackage.GetVector4();

            // Network object data
            ObjectData = dataPackage.GetPackage();
        }
    }
}