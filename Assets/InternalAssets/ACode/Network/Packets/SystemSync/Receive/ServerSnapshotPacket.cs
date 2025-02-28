using System.Collections.Generic;
using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Game.Snapshot.Receive;

namespace ProjectOlog.Code.Networking.Packets.SystemSync.Receive
{
    public class ServerSnapshotPacket: INetPackageSerializable
    {
        // Служебная информация от сервера
        public uint LastServerTick;
        public double LastServerTime;
        public uint LastClientReceivedTick;
        public ESnapshotBroadcastType BroadcastType; // Если глобальная - то delta не применяется.

        public List<SnapshotPlayerData> PlayersData = new();
        public List<SnapshotObjectData> ObjectsData = new();
        
        public void AddPlayerData(byte userID, byte[] playerData)
        {
            var playerTransformData = new SnapshotPlayerData
            {
                UserID = userID,
                TransformData = playerData 
            };
    
            PlayersData.Add(playerTransformData);
        }
        
        public void AddObjectData(ushort serverID, byte[] objectData)
        {
            var objectTransformData = new SnapshotObjectData
            {
                ServerID = serverID,
                TransformData = objectData 
            };
    
            ObjectsData.Add(objectTransformData);
        }
        
        public NetDataPackage GetPackage()
        {
            // Подготовка для сериализации игроков

            return new NetDataPackage(
                LastServerTick,
                LastServerTime,
                LastClientReceivedTick,
                (byte)BroadcastType,
                PlayersData.ToArray(),
                ObjectsData.ToArray()
            );
        }

        public void Deserialize(NetDataPackage dataPackage)
        {
            LastServerTick = dataPackage.GetUInt();
            LastServerTime = dataPackage.GetDouble();
            LastClientReceivedTick = dataPackage.GetUInt();
            BroadcastType = (ESnapshotBroadcastType)dataPackage.GetByte();
            
            // Десериализуем игроков
            PlayersData = new List<SnapshotPlayerData>(dataPackage.GetHeadlessCustomArray<SnapshotPlayerData>());
            
            // Десериализуем обьекты
            ObjectsData = new List<SnapshotObjectData>(dataPackage.GetHeadlessCustomArray<SnapshotObjectData>());
        }
    }
}