using System;
using System.Collections.Generic;

namespace ProjectOlog.Code.Networking.Profiles.Users
{
    public class NetworkUsersContainer
    {
        public int UsersCount => UsersList.Count;

        public Action OnUsersUpdate;
        public Action<int> OnUserJoin;
        public Action<int> OnUserLeave;
        
        public readonly List<NetworkUserData> UsersList;
        
        public NetworkUsersContainer()
        {
            UsersList = new List<NetworkUserData>();

            ClearContainer();
        }
        
        public void ClearContainer()
        {
            UsersList.Clear();
        }

        public void AddUser(NetworkUserData userData)
        {
            UsersList.Add(userData);
            OnUserJoin?.Invoke(userData.UserID);
        }
        
        public bool RemoveUser(int id)
        {
            foreach (var user in UsersList)
            {
                if (user.UserID == id)
                {
                    OnUserLeave?.Invoke(user.UserID);
                    UsersList.Remove(user);
                    
                    return true;
                }
            }
            
            return false;
        }

        public NetworkUserData GetUserDataByID(int id)
        {
            foreach (var user in UsersList)
            {
                if (user.UserID == id)
                {
                    return user;
                }
            }

            return null;
        }
        
        public bool TryGetUserDataByID(int id, out NetworkUserData userData)
        {
            userData = GetUserDataByID(id);

            return userData is not null;
        }
    }
}