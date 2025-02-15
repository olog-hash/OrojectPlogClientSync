using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets
{
    public class ServerInitializedPacket : INetPackageSerializable
    {
        // Базовая информация о сервере
        public uint Tickrate;
        
        public InitUserPacket[] InitUsers;
        public InitPlayerPacket[] InitPlayers;
        public InitObjectPacket[] InitObjects;

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(Tickrate, InitUsers, InitPlayers, InitObjects);
        }
        
        public void Deserialize(NetDataPackage dataPackage)
        {
            Tickrate = dataPackage.GetUInt();
            
            InitUsers = dataPackage.GetCustomArray<InitUserPacket>();
            InitPlayers = dataPackage.GetCustomArray<InitPlayerPacket>();
            InitObjects = dataPackage.GetCustomArray<InitObjectPacket>();
        }
    }
}