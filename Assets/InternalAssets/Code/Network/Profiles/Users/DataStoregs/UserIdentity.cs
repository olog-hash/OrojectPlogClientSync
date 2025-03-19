namespace ProjectOlog.Code.Network.Profiles.Users.DataStoregs
{
    public class UserIdentity
    {
        public byte ID { get; }
        public string Username { get; }
        public string UserToken { get; set; }
        public bool IsAdmin { get; }
        
        public UserIdentity(byte id, string username)
        {
            ID = id;
            Username = username;
        }
    }
}