using ProjectOlog.Code.Networking.Profiles.Users;

namespace ProjectOlog.Code.UI.HUD.TabPanel
{
    public class UserSlotData
    {
        public int ID;
        public string Username;
        public int KillCount;
        public bool IsDead;
        public int DeathCount;
        public int Ping;
        
        public UserSlotData(NetworkUserData userData)
        {
            ID = userData.UserID;
            Username = userData.Username;
            IsDead = userData.IsDead;
            DeathCount = userData.DeathCount;
            Ping = userData.Ping;
        }

        public void UpdateData(NetworkUserData userData)
        {
            Username = userData.Username;
            IsDead = userData.IsDead;
            DeathCount = userData.DeathCount;
            Ping = userData.Ping;
        }
    }
}