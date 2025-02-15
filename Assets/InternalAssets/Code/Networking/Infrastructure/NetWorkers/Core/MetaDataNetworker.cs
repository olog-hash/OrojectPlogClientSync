using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Networking.Infrastructure.Core;
using ProjectOlog.Code.Networking.Packets;
using UnityEngine;

namespace ProjectOlog.Code.Networking.Infrastructure.NetWorkers.Core
{
    public sealed class MetaDataNetworker : NetWorkerClient
    {
        [NetworkCallback]
        private void SyncTabPlayersData(NetPeer peer, NetDataPackage dataPackage)
        {
            var usersNetworkState = dataPackage.GetCustomArray<UserNetworkStateData>();
        
            // Обновляем данные по ID пользователя
            foreach (var networkUserState in usersNetworkState)
            {
                if (_usersContainer.TryGetUserDataByID(networkUserState.UserID, out var userData))
                {
                    // Обновляем все мета-данные
                    userData.Ping = networkUserState.Ping;
                    //userData.IsDead = networkUserState.IsDead; 
                    //userData.DeathCount = networkUserState.DeathCount;
                }
            }

            // Уведомляем UI об обновлении данных
            _usersContainer.OnUsersUpdate?.Invoke();
        }
    }
}