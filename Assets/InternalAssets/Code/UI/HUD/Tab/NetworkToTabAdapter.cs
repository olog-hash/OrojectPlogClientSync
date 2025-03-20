using System;
using System.Collections.Generic;
using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Network.Profiles.Users;
using ProjectOlog.Code.UI.HUD.Tab.Models;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using R3;

namespace ProjectOlog.Code.UI.HUD.Tab
{
    public class NetworkToTabAdapter : IDisposable
    {
        private readonly NetworkUsersContainer _usersContainer;
        private readonly TabViewModel _tabViewModel;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        // Словарь для отслеживания связи между сетевыми пользователями и их представлениями в Tab
        private readonly Dictionary<byte, PlayerViewModel> _playerViewModels = new Dictionary<byte, PlayerViewModel>();

        public NetworkToTabAdapter(NetworkUsersContainer usersContainer, TabViewModel tabViewModel)
        {
            _usersContainer = usersContainer;
            _tabViewModel = tabViewModel;

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
                _tabViewModel.RemovePlayer(playerViewModel);
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
            var playerViewModel = _tabViewModel.AddPlayer(userData.Username,
                userData.GameState.TeamID.Value,
                isLocalPlayer);

            // Сохраняем связь ID пользователя с его ViewModel
            _playerViewModels[userData.ID] = playerViewModel;

            // Подписываемся на изменения в UserGameState
            SubscribeToUserDataChanges(userData, playerViewModel);
        }

        private void SubscribeToUserDataChanges(NetworkUserData userData, PlayerViewModel playerViewModel)
        {
            // Синхронизируем данные между UserGameState и PlayerViewModel
            userData.GameState.TeamID
                .Subscribe(teamId => playerViewModel.ChangeTeam(teamId))
                .AddTo(_disposables);

            userData.GameState.IsDead
                .Subscribe(isDead => playerViewModel.IsDead.Value = isDead)
                .AddTo(_disposables);

            userData.GameState.Kills
                .Subscribe(kills => playerViewModel.Kills.Value = kills)
                .AddTo(_disposables);

            userData.GameState.Deaths
                .Subscribe(deaths => playerViewModel.Deaths.Value = deaths)
                .AddTo(_disposables);

            userData.GameState.Assists
                .Subscribe(assists => playerViewModel.Assists.Value = assists)
                .AddTo(_disposables);

            userData.GameState.Ping
                .Subscribe(ping => playerViewModel.Ping.Value = ping)
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