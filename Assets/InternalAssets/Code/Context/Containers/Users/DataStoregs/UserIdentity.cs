namespace ProjectOlog.Code.Network.Profiles.Users.DataStoregs
{
    public class UserIdentity
    {
        public byte ID { get; }
        public string Username { get; }
        public bool IsAdmin { get; }
        
        public UserIdentity(byte id, string username, bool isAdmin)
        {
            ID = id;
            Username = username;
            IsAdmin = isAdmin;
        }
    }
}