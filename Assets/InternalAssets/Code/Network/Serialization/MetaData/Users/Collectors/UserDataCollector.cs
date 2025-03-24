using ProjectOlog.Code.Network.Packets.SubPackets.Users.Components;
using ProjectOlog.Code.Network.Profiles.Users.DataStoregs;

namespace ProjectOlog.Code.Network.Serialization.MetaData.Users.Collectors
{
    public sealed class UserDataCollector
    {
        public UserIdentity CollectUserIdentity(UserIdentityData identityData)
        {
            var userIdentity = new UserIdentity(identityData.UserID, identityData.Username,
                identityData.IsAdmin);

            return userIdentity;
        }

        public UserGameState CollectUserGameState(UserGameStateData gameStateData)
        {
            var userGameState = new UserGameState();

            userGameState.IsDead.Value = gameStateData.IsDead;
            userGameState.Deaths.Value = gameStateData.Deaths;
            userGameState.Ping.Value = gameStateData.Ping;

            return userGameState;
        }
    }
}