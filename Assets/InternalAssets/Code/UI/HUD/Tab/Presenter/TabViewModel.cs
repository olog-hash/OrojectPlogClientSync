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
    /// <summary>
    /// Главная модель-представление для табло.
    /// Координирует работу всех компонентов табло (матч, статистика, игроки, команды)
    /// и обеспечивает интерфейс для взаимодействия с UI.
    /// </summary>
    public class TabViewModel : LayerViewModel
    {
        public MatchInfoModel MatchInfoModel { get; } = new MatchInfoModel();
        public PlayerStatsModel PlayerStatsModel { get; } = new PlayerStatsModel();
        public TeamsManagerModel TeamsManagerModel { get; } = new TeamsManagerModel();
        
        
        private NetworkToTabAdapter _networkToTabAdapter;

        public TabViewModel(NetworkUsersContainer usersContainer) : base()
        {
            _networkToTabAdapter = new NetworkToTabAdapter(usersContainer, TeamsManagerModel);

            SetupInitialData();
        }

        private void SetupInitialData()
        {
            // Устанавливаем информацию о матче
            MatchInfoModel.SetMatchInfo(
                "2х2 не мешать!", 
                "Сервер 7 Москва (11 - 20 уровень)",
                "Ангар. задний двор.",
                "(CD) Захват флага"
            );
        
            // Устанавливаем статистику игрока
            PlayerStatsModel.SetLocalPlayerStats(17, 9000, 97, 10000);
            
            // Создание команд
            TeamsManagerModel.AddTeam(1, "Игроки", 5);
        }
    
        public override void Dispose()
        {
            base.Dispose();
    
            MatchInfoModel.Dispose();
            PlayerStatsModel.Dispose();
            TeamsManagerModel.Dispose();
            
            _networkToTabAdapter.Dispose();
        }
    }
}