using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.SystemSync.Send
{
    public class ClientSystemSyncPacket : INetPackageSerializable
    {
        public uint LastKnownServerTick;

        // Остаточные данные
        public ClientSyncPlayerStateData PlayerStateData;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(LastKnownServerTick, PlayerStateData);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            LastKnownServerTick = dataPackage.GetUInt();
            PlayerStateData = dataPackage.GetCustom<ClientSyncPlayerStateData>();
        }
    }
}