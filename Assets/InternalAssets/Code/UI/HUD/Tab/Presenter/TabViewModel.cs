using System;
using ObservableCollections;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Network.Profiles.Users;
using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.Tab.Models;
using R3;
using UnityEngine;
using Random = System.Random;

namespace ProjectOlog.Code.UI.HUD.Tab.Presenter
{
    public class TabViewModel : BaseViewModel, ILayer
    {
        public ReactiveProperty<string> RoomName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ServerName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> MapName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ModeName { get; } = new ReactiveProperty<string>();

        public ObservableList<TeamViewModel> Teams { get; } = new ObservableList<TeamViewModel>();
        public ObservableList<PlayerViewModel> Players { get; } = new ObservableList<PlayerViewModel>();

        public ReactiveProperty<int> PlayerLevel { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> PlayerCurrentExperience { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> PlayerNextLevelExperience { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> PlayerExperienceGained { get; } = new ReactiveProperty<int>();
    
        public Subject<PlayerViewModel> PlayerTeamChanged { get; } = new Subject<PlayerViewModel>();
        
        private NetworkToTabAdapter _networkToTabAdapter;

        public TabViewModel(NetworkUsersContainer usersContainer) : base()
        {
            _networkToTabAdapter = new NetworkToTabAdapter(usersContainer, this);

            SetupInitialData();
        }

        private void SetupInitialData()
        {
            // Устанавливаем информацию о матче
            this.SetMatchInfo(
                "2х2 не мешать!", 
                "Сервер 7 Москва (11 - 20 уровень)",
                "Ангар. задний двор.",
                "(CD) Захват флага"
            );
        
            // Устанавливаем статистику игрока
            this.SetPlayerStats(17, 9000, 97, 10000);
            
            // Создание команд
            this.AddTeam(1, "Игроки", 5);
        }
        
        public void SetMatchInfo(string roomName, string serverName, string mapName, string modeName)
        {
            RoomName.Value = roomName;
            ServerName.Value = serverName;
            MapName.Value = mapName;
            ModeName.Value = modeName;
        }

        public TeamViewModel AddTeam(int teamID, string teamName, int maxPlayers, Color teamColor)
        {
            var team = new TeamViewModel(teamID, teamName, maxPlayers, this, teamColor);
            Teams.Add(team);
            return team;
        }

        // Перегрузка для обратной совместимости
        public TeamViewModel AddTeam(int teamID, string teamName, int maxPlayers)
        {
            var defaultColor = new Color(0x85/255f, 0x82/255f, 0x7E/255f);
            
            return AddTeam(teamID, teamName, maxPlayers, defaultColor);
        }

        public void SetPlayerStats(int level, int experience, int experienceGained, int experienceLeft)
        {
            PlayerLevel.Value = level;
            PlayerCurrentExperience.Value = experience;
            PlayerExperienceGained.Value = experienceGained;
            PlayerNextLevelExperience.Value = experienceLeft;
        }

        // Добавляем методы для работы с игроками
        public PlayerViewModel AddPlayer(string name, int teamID, bool isLocal = false)
        {
            var player = new PlayerViewModel(name, teamID, isLocal);
            
            // Подписываемся на изменение команды
            player.TeamID.Subscribe(_ => PlayerTeamChanged.OnNext(player))
                .AddTo(_disposables);
            
            Players.Add(player);
            return player;
        }
    
        public void RemovePlayer(PlayerViewModel player)
        {
            Players.Remove(player);
        }
    
        public override void Dispose()
        {
            base.Dispose();
    
            foreach (var player in Players)
                player.Dispose();
            Players.Clear();
    
            foreach (var team in Teams)
                team.Dispose();
            Teams.Clear();
        
            RoomName.Dispose();
            ServerName.Dispose();
            MapName.Dispose();
            ModeName.Dispose();
            PlayerLevel.Dispose();
            PlayerCurrentExperience.Dispose();
            PlayerExperienceGained.Dispose();
            PlayerNextLevelExperience.Dispose();
        
            PlayerTeamChanged.Dispose();
            
            _networkToTabAdapter.Dispose();
        }

        public void ShowLayer() => Show();

        public void HideLayer() => Hide();
    }
}