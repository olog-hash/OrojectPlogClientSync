using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Packets.SubPackets.Instantiate;

namespace ProjectOlog.Code.Network.Packets
{
    public class ServerInitializedPacket : INetPackageSerializable
    {
        // Базовая информация о сервере
        public uint Tickrate;
        
        public InitUserPacket[] InitUsers;
        public InstantiatePlayerPacket InstantiatePlayerPacket;
        public InstantiateObjectPacket InstantiateObjectPacket;

        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(Tickrate, InitUsers, InstantiatePlayerPacket, InstantiateObjectPacket);
        }
        
        public void Deserialize(NetDataPackage dataPackage)
        {
            Tickrate = dataPackage.GetUInt();
            
            InitUsers = dataPackage.GetCustomArray<InitUserPacket>();
            InstantiatePlayerPacket = dataPackage.GetCustom<InstantiatePlayerPacket>();
            InstantiateObjectPacket = dataPackage.GetCustom<InstantiateObjectPacket>();
        }
    }
}