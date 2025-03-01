using System;
using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Network.Profiles.Users;
using ProjectOlog.Code.UI.Core;

namespace ProjectOlog.Code.UI.HUD.TabPanel
{
    public class TabViewModel: BaseViewModel, ILayer
    {
        public Action<bool> OnShowHideChanged;
        public Action OnUsersListChanged;
        public Action OnUserStateUpdate;

        public readonly List<UserSlotData> UsersData;
        
        private NetworkUsersContainer _usersContainer;

        public TabViewModel(NetworkUsersContainer usersContainer)
        {
            _usersContainer = usersContainer;
            
            OnShowHideChanged = null;
            OnUsersListChanged = null;
            OnUserStateUpdate = null;

            UsersData = new List<UserSlotData>();

            Initialize();
        }

        private void Initialize()
        {
            foreach (var userData in _usersContainer.UsersList)
            {
                AddUser(userData.ID);
            }
            
            _usersContainer.OnUserJoin += AddUser;
            _usersContainer.OnUserLeave += RemoveUser;
            _usersContainer.OnUsersUpdate += UpdateUsers;
            
            // TODO подумать об отписках
        }

        public void AddUser(byte id)
        {
            bool isAlreadyExist = UsersData.Any(userData => userData.ID == id);
            if (isAlreadyExist)
            {
                return;
            }
            
            var networkPlayerData = _usersContainer.GetUserDataByID(id);
            var userSlotData = new UserSlotData(networkPlayerData);
            
            UsersData.Add(userSlotData);
            
            OnUsersListChanged?.Invoke();
        }

        public void RemoveUser(byte userID)
        {
            for (int i = 0; i < UsersData.Count; i++)
            {
                if (UsersData[i].ID == userID)
                {
                    UsersData.RemoveAt(i);
                    OnUsersListChanged?.Invoke();
                    
                    break;
                }
            }
        }

        public void UpdateUsers()
        {
            if (UsersData.Count == _usersContainer.UsersCount)
            {
                bool wasStatusChanged = false;
                
                for (int i = 0; i < UsersData.Count; i++)
                {
                    var userData = UsersData[i];
                    var networkUserData = _usersContainer.GetUserDataByID(userData.ID);

                    if (networkUserData != null)
                    {
                        wasStatusChanged = true;

                        userData.UpdateData(networkUserData);
                    }
                }

                if (wasStatusChanged)
                {
                    OnUserStateUpdate?.Invoke();
                }
            }
        }

        public void OnShow()
        {
            OnShowHideChanged?.Invoke(true);
        }

        public void OnHide()
        {
            OnShowHideChanged?.Invoke(false);
        }
    }
}