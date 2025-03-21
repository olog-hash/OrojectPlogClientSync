using System;
using System.Collections.Generic;
using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Network.Profiles.Users;
using ProjectOlog.Code.UI.HUD.Tab.Models;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using R3;

namespace ProjectOlog.Code.UI.HUD.Tab
{
    /// <summary>
    /// Адаптер, связывающий сетевую подсистему с интерфейсом табло.
    /// Преобразует сетевые данные о пользователях в модели игроков для отображения,
    /// отслеживает присоединение и отключение игроков, и обновляет их статистику.
    /// </summary>
    public class NetworkToTabAdapter : IDisposable
    {
        private readonly NetworkUsersContainer _usersContainer;
        private readonly TeamsManagerModel _TeamsManagerModel;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        // Словарь для отслеживания связи между сетевыми пользователями и их представлениями в Tab
        private readonly Dictionary<byte, PlayerModel> _playerViewModels = new Dictionary<byte, PlayerModel>();

        public NetworkToTabAdapter(NetworkUsersContainer usersContainer, TeamsManagerModel teamsManagerModel)
        {
            _usersContainer = usersContainer;
            _TeamsManagerModel = teamsManagerModel;

            // Подписываемся на события контейнера пользователей
            _usersContainer.OnUserJoin += OnUserJoin;
            _usersContainer.OnUserLeave += OnUserLeave;
            _usersContainer.OnUsersUpdate += OnUsersUpdate;

            // Инициализируем текущее состояние
            InitializeCurrentUsers();
        }

        private void InitializeCurrentUsers()
        {
            foreach (var userData in _usersContainer.UsersList)
            {
                AddPlayerViewModel(userData);
            }
        }

        private void OnUserJoin(byte userId)
        {
            var userData = _usersContainer.GetUserDataByID(userId);
            if (userData != null)
            {
                AddPlayerViewModel(userData);
            }
        }

        private void OnUserLeave(byte userId)
        {
            if (_playerViewModels.TryGetValue(userId, out var playerViewModel))
            {
                _TeamsManagerModel.RemovePlayer(playerViewModel);
                _playerViewModels.Remove(userId);
            }
        }

        private void OnUsersUpdate()
        {
            // Обновляем данные для всех игроков при необходимости
        }

        private void AddPlayerViewModel(NetworkUserData userData)
        {
            // Определяем, локальный ли это игрок
            bool isLocalPlayer = userData.ID == LocalData.LocalUserID;

            // Создаем ViewModel для игрока и добавляем в Tab
            var playerViewModel = _TeamsManagerModel.AddPlayer(userData.Username,
                userData.GameState.TeamID.Value,
                isLocalPlayer);

            // Сохраняем связь ID пользователя с его ViewModel
            _playerViewModels[userData.ID] = playerViewModel;

            // Подписываемся на изменения в UserGameState
            SubscribeToUserDataChanges(userData, playerViewModel);
        }

        private void SubscribeToUserDataChanges(NetworkUserData userData, PlayerModel playerModel)
        {
            // Синхронизируем данные между UserGameState и PlayerViewModel
            userData.GameState.TeamID
                .Subscribe(teamId => playerModel.ChangeTeam(teamId))
                .AddTo(_disposables);

            userData.GameState.IsDead
                .Subscribe(isDead => playerModel.IsDead.Value = isDead)
                .AddTo(_disposables);

            userData.GameState.Kills
                .Subscribe(kills => playerModel.Kills.Value = kills)
                .AddTo(_disposables);

            userData.GameState.Deaths
                .Subscribe(deaths => playerModel.Deaths.Value = deaths)
                .AddTo(_disposables);

            userData.GameState.Assists
                .Subscribe(assists => playerModel.Assists.Value = assists)
                .AddTo(_disposables);

            userData.GameState.Ping
                .Subscribe(ping => playerModel.Ping.Value = ping)
                .AddTo(_disposables);
        }

        public void Dispose()
        {
            // Отписываемся от событий
            _usersContainer.OnUserJoin -= OnUserJoin;
            _usersContainer.OnUserLeave -= OnUserLeave;
            _usersContainer.OnUsersUpdate -= OnUsersUpdate;

            _disposables.Dispose();
            _playerViewModels.Clear();
        }
    }
}