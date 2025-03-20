using R3;

namespace ProjectOlog.Code.Network.Profiles.Users.DataStoregs
{
    public class UserGameState
    {
        public ReactiveProperty<int> TeamID { get; } = new ReactiveProperty<int>(1);
        public ReactiveProperty<int> TeamRang { get; } = new ReactiveProperty<int>();

        public ReactiveProperty<bool> IsDead { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<int> TotalScore { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Kills { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Assists { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Deaths { get; } = new ReactiveProperty<int>();
        
        public ReactiveProperty<short> Ping { get; } = new ReactiveProperty<short>();

        public void DeathUser(bool isFakeDeath = false)
        {
            if (!isFakeDeath)
            {
                IsDead.Value = true;
            }

            Deaths.Value++;
        }

        public void ReviveUser()
        {
            IsDead.Value = false;
        }
    }
}