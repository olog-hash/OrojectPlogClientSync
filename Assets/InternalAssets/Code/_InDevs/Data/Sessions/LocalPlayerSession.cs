using ProjectOlog.Code.Core.Enums;

namespace ProjectOlog.Code._InDevs.Data.Sessions
{
    public class LocalPlayerSession
    {
        // Data
        public ENetworkObjectType CurrentSpawnObjectID;

        public LocalPlayerSession()
        {
            Reset();
        }

        public void Reset()
        {
            CurrentSpawnObjectID = 0;
        }
    }
}
