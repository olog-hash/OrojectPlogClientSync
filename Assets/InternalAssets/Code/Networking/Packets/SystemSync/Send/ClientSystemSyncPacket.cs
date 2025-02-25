using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.SystemSync.Send
{
    public class ClientSystemSyncPacket : INetPackageSerializable
    {
        public uint LastKnownServerTick;
        public ushort LastStateVersion;
        public double RemoteTime;

        // Остаточные данные
        public ClientSyncPlayerStateData PlayerStateData;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(LastKnownServerTick, LastStateVersion,RemoteTime, PlayerStateData);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            LastKnownServerTick = dataPackage.GetUInt();
            LastStateVersion = dataPackage.GetUShort();
            RemoteTime = dataPackage.GetDouble();
            PlayerStateData = dataPackage.GetCustom<ClientSyncPlayerStateData>();
        }
    }
}