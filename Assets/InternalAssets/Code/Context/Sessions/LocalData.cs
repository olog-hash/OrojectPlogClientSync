using ProjectOlog.Code.DataStorage.Core;

namespace ProjectOlog.Code._InDevs.Data.Sessions
{
    public class LocalData : ISceneContainer
    {
        public static LocalData Instance { get; private set; }
        
        public static byte LocalID;
        public static string LocalName;

        void ISceneContainer.Reset()
        {
            Instance = this;
            
            LocalID = 0;
            LocalName = string.Empty;
        }
    }
}