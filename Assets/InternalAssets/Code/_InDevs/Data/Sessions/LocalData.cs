namespace ProjectOlog.Code._InDevs.Data.Sessions
{
    public class LocalData
    {
        public static int LocalID;
        public static string LocalName;

        public LocalData()
        {
            Initialize();
        }

        public static void Initialize()
        {
            LocalID = -1;
            LocalName = string.Empty;
        }
    }
}