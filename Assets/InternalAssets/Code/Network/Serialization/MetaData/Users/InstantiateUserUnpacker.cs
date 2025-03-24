using System.Collections.Generic;
using ProjectOlog.Code.Network.Packets.SubPackets.Users;
using ProjectOlog.Code.Network.Packets.SubPackets.Users.Components;
using ProjectOlog.Code.Network.Profiles.Users;
using ProjectOlog.Code.Network.Serialization.MetaData.Users.Collectors;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Network.Serialization.MetaData.Users
{
    /// <summary>
    /// Распаковка пакета содержащий данные об пользователях
    /// </summary>
    public sealed class InstantiateUserUnpacker
    {
        private UserDataCollector _userDataCollector = new();

        /// <summary>
        /// Распаковка пакета, содержащего данные о пользователях на сервере.
        /// </summary>
        public NetworkUserData[] UnpackInstantiateUsersPacket(InstantiateUserPacket packet)
        {
            var usersList = new List<NetworkUserData>();
            
            foreach (var userDataPacket in packet.UserDataPackets)
            {
                var userData = UnpackUserData(userDataPacket);
                
                // Добавляем распакованного пользователя в список.
                usersList.Add(userData);
            }

            return usersList.ToArray();
        }

        private NetworkUserData UnpackUserData(UserDataPacket userDataPacket)
        {
            var identity = _userDataCollector.CollectUserIdentity(userDataPacket.IdentityData);
            var gameState = _userDataCollector.CollectUserGameState(userDataPacket.GameStateData);

            // Собираем итогового пользователя
            var userData = new NetworkUserData(identity, gameState);
            return userData;
        }
    }
}