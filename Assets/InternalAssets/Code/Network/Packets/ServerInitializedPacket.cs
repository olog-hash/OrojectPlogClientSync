using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Instantiate;
using ProjectOlog.Code.Network.Packets.SubPackets.Users;

namespace ProjectOlog.Code.Network.Packets
{
    public class ServerInitializedPacket : INetPackageSerializable
    {
        // Базовая информация о сервере
        public uint Tickrate;
        
        public InstantiateUserPacket InitUsers;
        public InstantiatePlayerPacket InstantiatePlayerPacket;
        public InstantiateObjectPacket InstantiateObjectPacket;

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(Tickrate, InitUsers, InstantiatePlayerPacket, InstantiateObjectPacket);
        }
        
        public void Deserialize(NetDataPackage dataPackage)
        {
            Tickrate = dataPackage.GetUInt();
            
            InitUsers = dataPackage.GetCustom<InstantiateUserPacket>();
            InstantiatePlayerPacket = dataPackage.GetCustom<InstantiatePlayerPacket>();
            InstantiateObjectPacket = dataPackage.GetCustom<InstantiateObjectPacket>();
        }
    }
}