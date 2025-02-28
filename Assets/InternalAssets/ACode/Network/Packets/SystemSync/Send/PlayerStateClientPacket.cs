using LiteNetLib.Utils;

namespace ProjectOlog.Code.Networking.Packets.SystemSync.Send
{
    /// <summary>
    /// Сетевой пакет, отправляемый клиентом на сервер, содержащий
    /// информацию о состоянии игрока и системные данные для синхронизации.
    /// </summary>
    public class PlayerStateClientPacket : INetPackageSerializable
    {
        public uint LastKnownServerTick;
        public ushort LastStateVersion;
        public double RemoteTime;

        // Остаточные данные
        public PlayerTransformStateData PlayerTransformStateData;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(LastKnownServerTick, LastStateVersion,RemoteTime, PlayerTransformStateData);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            LastKnownServerTick = dataPackage.GetUInt();
            LastStateVersion = dataPackage.GetUShort();
            RemoteTime = dataPackage.GetDouble();
            PlayerTransformStateData = dataPackage.GetCustom<PlayerTransformStateData>();
        }
    }
}