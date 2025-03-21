using System;
using R3;

namespace ProjectOlog.Code.UI.HUD.Tab.Models
{
    /// <summary>
    /// Представляет отдельного игрока в системе табло.
    /// Отслеживает статистику игрока (убийства, смерти, помощь),
    /// принадлежность к команде и позицию в рейтинге команды.
    /// </summary>
    public class PlayerModel : IDisposable
    {
        public ReactiveProperty<int> TeamID { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> TeamRang { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<bool> IsLocal { get; } = new ReactiveProperty<bool>();

        public ReactiveProperty<bool> IsDead { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> Name { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<int> TotalScore { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Kills { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Assists { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Deaths { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Ping { get; } = new ReactiveProperty<int>();
    
        private CompositeDisposable _disposables = new CompositeDisposable();

        public PlayerModel(string name, int teamID, bool isLocal = false)
        {
            Name.Value = name;
            TeamID.Value = teamID;
            IsLocal.Value = isLocal;
    
            Observable.CombineLatest(Kills, Deaths, Assists)
                .Subscribe(_ => UpdateScore())
                .AddTo(_disposables);
        }

        public void UpdateStats(int kills, int deaths, int assists)
        {
            Kills.Value = kills;
            Deaths.Value = deaths;
            Assists.Value = assists;
        }

        private void UpdateScore()
        {
            TotalScore.Value = Math.Max(0, Kills.Value + Assists.Value - Deaths.Value);
        }

        public void ChangeTeam(int newTeamID)
        {
            if (TeamID.Value != newTeamID)
                TeamID.Value = newTeamID;
        }

        public void Dispose()
        {
            _disposables.Dispose();
            
            IsDead.Dispose();
            IsLocal.Dispose();
            TeamID.Dispose();
            TeamRang.Dispose();
            TotalScore.Dispose();
            Name.Dispose();
            Kills.Dispose();
            Assists.Dispose();
            Deaths.Dispose();
            Ping.Dispose();
        }
    }
}