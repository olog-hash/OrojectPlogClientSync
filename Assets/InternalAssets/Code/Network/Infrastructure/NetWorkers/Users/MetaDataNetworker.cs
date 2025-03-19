using LiteNetLib;
using LiteNetLib.Utils;
using ProjectOlog.Code.Network.Infrastructure.Core;
using ProjectOlog.Code.Network.Packets;

namespace ProjectOlog.Code.Network.Infrastructure.NetWorkers.Users
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
                    userData.GameState.Ping.Value = networkUserState.Ping;
                }
            }

            // Уведомляем UI об обновлении данных
            _usersContainer.OnUsersUpdate?.Invoke();
        }
    }
}