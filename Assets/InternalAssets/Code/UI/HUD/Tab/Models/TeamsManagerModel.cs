using System;
using ObservableCollections;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Tab.Models
{
    /// <summary>
    /// Центральный менеджер игроков и команд.
    /// Управляет коллекциями игроков и команд, обеспечивая связь между ними
    /// и предоставляя методы для добавления/удаления игроков и команд.
    /// </summary>
    public class TeamsManagerModel : IDisposable
    {
        public ObservableList<TeamModel> Teams { get; } = new ObservableList<TeamModel>();
        public ObservableList<PlayerModel> Players { get; } = new ObservableList<PlayerModel>();
        
        public Subject<PlayerModel> PlayerTeamChanged { get; } = new Subject<PlayerModel>();
        
        private CompositeDisposable _disposables = new CompositeDisposable();

        public TeamModel AddTeam(int teamID, string teamName, int maxPlayers, Color teamColor)
        {
            var team = new TeamModel(teamID, teamName, maxPlayers, this, teamColor);
            Teams.Add(team);
            return team;
        }

        // Перегрузка для обратной совместимости
        public TeamModel AddTeam(int teamID, string teamName, int maxPlayers)
        {
            var defaultColor = new Color(0x85/255f, 0x82/255f, 0x7E/255f);
            
            return AddTeam(teamID, teamName, maxPlayers, defaultColor);
        }

        // Добавляем методы для работы с игроками
        public PlayerModel AddPlayer(string name, int teamID, bool isLocal = false)
        {
            var player = new PlayerModel(name, teamID, isLocal);
            
            // Подписываемся на изменение команды
            player.TeamID.Subscribe(_ => PlayerTeamChanged.OnNext(player))
                .AddTo(_disposables);
            
            Players.Add(player);
            return player;
        }
    
        public void RemovePlayer(PlayerModel player)
        {
            Players.Remove(player);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
            
            foreach (var player in Players)
                player.Dispose();
            Players.Clear();
    
            foreach (var team in Teams)
                team.Dispose();
            Teams.Clear();
        
            PlayerTeamChanged.Dispose();
        }
    }
}