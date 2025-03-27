using ProjectOlog.Code.Network.Profiles.Users.DataStoregs;

namespace ProjectOlog.Code.Network.Profiles.Users
{
    public class NetworkUserData
    {
        public byte ID => Identity.ID;
        public short Ping => GameState.Ping.Value;
        public string Username => Identity.Username;

        // Хранилища
        public readonly UserIdentity Identity;
        public readonly UserGameState GameState;

        public NetworkUserData(UserIdentity identity, UserGameState gameState)
        {
            Identity = identity;
            GameState = gameState;
        }
    }
}