using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code._InDevs.Players.Init;
using ProjectOlog.Code._InDevs.Players.Respawn;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using ProjectOlog.Code.Networking.Packets;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Players
{
    public sealed class BasicPlayerNetworker : NetWorkerClient
    {
        public void SpawnPlayerRequest()
        {
            SendTo(nameof(SpawnPlayerRequest), new NetDataPackage(), DeliveryMethod.ReliableOrdered);
        }
        
        [NetworkCallback]
        private void InitPlayer(NetPeer peer, NetDataPackage dataPackage)
        {
            var playerDataCached = new InitPlayerPacket();
            playerDataCached.Deserialize(dataPackage);
            
            if (!_usersContainer.TryGetUserDataByID(playerDataCached.UserID, out var userData)) return;
            bool isLocalPlayer = LocalData.LocalID == playerDataCached.UserID;

            var initPlayerEvent = new InitPlayerEvent
            {
                ServerID = playerDataCached.ServerID,
                UserID = playerDataCached.UserID,
                Username = userData?.Username != null ? userData.Username : "NONE",

                Position = playerDataCached.Position,
                Rotation = playerDataCached.Rotation,
                IsDead = playerDataCached.IsDead,
                IsLocalPlayer = isLocalPlayer,
            };
            
            World.Default.CreateTickEvent().AddComponentData(initPlayerEvent);
        }
        
        [NetworkCallback]
        private void RespawnPlayer(NetPeer peer, NetDataPackage dataPackage)
        {
            byte userID = dataPackage.GetByte();
            var position = dataPackage.GetVector3();
            var rotation = dataPackage.GetVector4();

            if (!_entitiesContainer.PlayerEntities.TryGetPlayerEntity(userID, out var playerProvider)) return;
            
            var respawnPlayerEvent = new RespawnPlayerEvent
            {
                PlayerProvider = playerProvider,
                Position = position,
                Rotation = rotation,
            };
            
            World.Default.CreateTickEvent().AddComponentData(respawnPlayerEvent);
        }

        public void RespawnPlayerInfoReceived()
        {
            SendTo(nameof(RespawnPlayerInfoReceived), new NetDataPackage(), DeliveryMethod.ReliableOrdered);
        }
    }
}