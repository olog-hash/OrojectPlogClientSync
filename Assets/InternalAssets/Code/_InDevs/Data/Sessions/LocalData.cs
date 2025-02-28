namespace ProjectOlog.Code._InDevs.Data.Sessions
{
    public class LocalData
    {
        public static byte LocalUserID;
        public static string LocalName;

        public LocalData()
        {
            Initialize();
        }

        public static void Initialize()
        {
            LocalUserID = 0;
            LocalName = string.Empty;
        }
    }
}