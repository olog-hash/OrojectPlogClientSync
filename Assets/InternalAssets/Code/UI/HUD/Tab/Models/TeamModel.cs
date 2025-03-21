using System;
using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Tab.Models
{
    /// <summary>
    /// Модель команды, управляющая группой игроков со сходной идентификацией.
    /// Отвечает за хранение информации о команде, отслеживание её игроков,
    /// расчёт рейтингов игроков внутри команды и подсчёт общего счёта команды.
    /// </summary>
    public class TeamModel : IDisposable
    {
        public ReactiveProperty<int> TeamID { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<string> TeamName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<Color> TeamColor { get; } = new ReactiveProperty<Color>(Color.white);
        public ReactiveProperty<int> MaxCount { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> TotalScore { get; } = new ReactiveProperty<int>();
        public ObservableList<PlayerModel> Players { get; } = new ObservableList<PlayerModel>();
        public ReactiveProperty<bool> TeamPlayersChanged { get; } = new ReactiveProperty<bool>(false);
    
        private CompositeDisposable _disposables = new CompositeDisposable();
        private TeamsManagerModel _TeamsManagerModel;

        public TeamModel(int teamID, string name, int maxCount, TeamsManagerModel teamsManagerModel, Color teamColor)
        {
            TeamID.Value = teamID;
            TeamName.Value = name;
            MaxCount.Value = maxCount;
            TeamColor.Value = teamColor;
            _TeamsManagerModel = teamsManagerModel;
            
            UpdateTeamPlayersList();
            
            var playerChangeEvents = Observable.Merge(
                _TeamsManagerModel.Players.ObserveAdd().Select(_ => Unit.Default),
                _TeamsManagerModel.Players.ObserveRemove().Select(_ => Unit.Default),
                _TeamsManagerModel.PlayerTeamChanged.Select(_ => Unit.Default)
            );
        
            playerChangeEvents.Subscribe(_ => UpdateTeamPlayersList()).AddTo(_disposables);
        }
        
        // Метод обновления списка игроков команды
        private void UpdateTeamPlayersList()
        {
            Players.Clear();
        
            // Добавляем всех игроков этой команды
            foreach (var player in _TeamsManagerModel.Players)
            {
                if (player.TeamID.Value == TeamID.Value)
                {
                    Players.Add(player);
                    
                    player.TotalScore
                        .Subscribe(_ => {
                            RecalculatePlayerRanks();
                            RecalculateScore();
                        })
                        .AddTo(_disposables);
                }
            }
        
            RecalculatePlayerRanks();
            RecalculateScore();
            NotifyTeamChanged();
        }
    
        private void RecalculatePlayerRanks()
        {
            var sortedByScore = Players
                .OrderByDescending(p => p.TotalScore.Value)
                .ThenBy(p => p.Name.Value)
                .ToList();
    
            for (int i = 0; i < sortedByScore.Count; i++)
                sortedByScore[i].TeamRang.Value = i + 1;
        
            NotifyTeamChanged();
        }
    
        private void RecalculateScore()
        {
            TotalScore.Value = Players.Sum(p => p.TotalScore.Value);
        }
    
        private void NotifyTeamChanged()
        {
            TeamPlayersChanged.Value = !TeamPlayersChanged.Value;
        }

        public void Dispose()
        {
            _disposables.Dispose();
            
            TeamID.Dispose();
            TeamName.Dispose();
            MaxCount.Dispose();
            TotalScore.Dispose();
            TeamPlayersChanged.Dispose();
        }
    }
}