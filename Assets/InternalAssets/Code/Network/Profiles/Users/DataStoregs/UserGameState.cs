using R3;

namespace ProjectOlog.Code.Network.Profiles.Users.DataStoregs
{
    public class UserGameState
    {
        public ReactiveProperty<int> TeamID { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<bool> IsDead { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<int> Kills { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Assists { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Deaths { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<short> Ping { get; } = new ReactiveProperty<short>();

        public void KillUser(bool isRealDeath = true)
        {
            if (isRealDeath)
            {
                IsDead.Value = true;
            }

            Deaths.Value++;
        }

        public void SetDeathUser()
        {
            IsDead.Value = true;
        }

        public void ReviveUser()
        {
            IsDead.Value = false;
        }
    }
}