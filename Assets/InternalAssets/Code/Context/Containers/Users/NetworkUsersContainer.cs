using System;
using System.Collections.Generic;
using ProjectOlog.Code.DataStorage.Core;

namespace ProjectOlog.Code.Network.Profiles.Users
{
     /// <summary>
    /// Контейнер всех пользователей, что подключены и авторизованы.
    /// </summary>
    public sealed class NetworkUsersContainer : ISceneContainer
    {
        // Свойство для доступа к списку (при необходимости)
        public IReadOnlyCollection<NetworkUserData> UsersList => _usersById.Values;
        public int UsersCount => UsersList.Count;
        
        // Основное хранилище по ID (O(1) доступ)
        private readonly Dictionary<byte, NetworkUserData> _usersById = new Dictionary<byte, NetworkUserData>();
        
        // Ивенты
        public Action OnUsersUpdate;
        public Action<byte> OnUserJoin;
        public Action<byte> OnUserLeave;

        public void Reset()
        {
            _usersById.Clear();

            OnUsersUpdate = null;
            OnUserJoin = null;
            OnUserLeave = null;
        }
        
        public void AddUser(NetworkUserData userData)
        {
            if (userData == null) return;
            
            // Добавляем пользователя во все словари
            _usersById[userData.ID] = userData;
            
            // Вызываем ивент
            OnUserJoin?.Invoke(userData.ID);
        }

        public bool RemoveUser(byte id)
        {
            if (!_usersById.TryGetValue(id, out var userData))
                return false;
                
            // Уведомляем перед удалением
            OnUserLeave?.Invoke(userData.ID);
            
            // Удаляем из всех словарей
            _usersById.Remove(id);
            return true;
        }

        public void UpdateLatency(byte id, int latency)
        {
            if (_usersById.TryGetValue(id, out var userData))
            {
                //userData.ConnectionState.UpdatePing(latency);
            }
        }

        public NetworkUserData GetUserDataByID(byte id)
        {
            return _usersById.TryGetValue(id, out var userData) ? userData : null;
        }

        public bool TryGetUserDataByID(byte id, out NetworkUserData userData)
        {
            return _usersById.TryGetValue(id, out userData);
        }
        
        public NetworkUserData GetUserDataByName(string userName)
        {
            foreach (var userData in _usersById.Values)
            {
                if (userData.Username == userName)
                {
                    return userData;
                }
            }
            return null;
        }

        public bool TryGetUserDataByName(string userName, out NetworkUserData userData)
        {
            userData = GetUserDataByName(userName);
            return userData != null;
        }
    }
}