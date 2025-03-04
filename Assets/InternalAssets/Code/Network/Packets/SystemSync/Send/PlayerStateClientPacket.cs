using LiteNetLib.Utils;

namespace ProjectOlog.Code.Network.Packets.SystemSync.Send
{
    /// <summary>
    /// Сетевой пакет, отправляемый клиентом на сервер, содержащий
    /// информацию о состоянии игрока и системные данные для синхронизации.
    /// </summary>
    public class PlayerStateClientPacket : INetPackageSerializable
    {
        public uint LastKnownServerTick;
        public double RemoteTime;

        // Остаточные данные
        public PlayerTransformStateData PlayerTransformStateData;
        
        public NetDataPackage GetPackage()
        {
            return new NetDataPackage(LastKnownServerTick,RemoteTime, PlayerTransformStateData);
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            LastKnownServerTick = dataPackage.GetUInt();
            RemoteTime = dataPackage.GetDouble();
            PlayerTransformStateData = dataPackage.GetCustom<PlayerTransformStateData>();
        }
    }
}