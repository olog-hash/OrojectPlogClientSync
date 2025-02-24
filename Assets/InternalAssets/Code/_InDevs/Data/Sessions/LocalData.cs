namespace ProjectOlog.Code._InDevs.Data.Sessions
{
    public class LocalData
    {
        public static int LocalUserID;
        public static string LocalName;

        public LocalData()
        {
            Initialize();
        }

        public static void Initialize()
        {
            LocalUserID = -1;
            LocalName = string.Empty;
        }
    }
}