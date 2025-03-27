using LeopotamGroup.Globals;
using ProjectOlog.Code.DataStorage.Core;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using ProjectOlog.Code.Network.Profiles.Entities;

namespace ProjectOlog.Code._InDevs.Data.Sessions
{
    public class LocalInventorySession
    {
        // Data
        public ENetworkObjectType CurrentSpawnObjectID = ENetworkObjectType.None;

        public LocalInventorySession()
        {
            CurrentSpawnObjectID = ENetworkObjectType.None;
        }
    }
}
