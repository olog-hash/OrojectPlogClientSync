using System.Collections.Generic;
using System.Linq;
using ObservableCollections;
using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.HUD.Tab.Models;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using ProjectOlog.Code.UI.HUD.Tab.View.Services;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.Tab.View.Controllers
{
    public class TeamsBlockController : UIToolkitElementView
    {
        private VisualElement _teamsContainer;
        private VisualTreeAsset _teamBlockTemplate;
        private VisualTreeAsset _playerSlotTemplate;
        private TeamHeightCalculator _heightCalculator;

        private TabViewModel _model;
        
        private Dictionary<int, VisualElement> _teamElements = new Dictionary<int, VisualElement>();
        
        public TeamsBlockController(VisualElement root, VisualTreeAsset teamTemplate, VisualTreeAsset playerTemplate) : base(root)
        {
            _teamBlockTemplate = teamTemplate;
            _playerSlotTemplate = playerTemplate;
            _heightCalculator = new TeamHeightCalculator();
        }

        protected override void SetVisualElements()
        {
            _teamsContainer = Root.Q<VisualElement>("teams-block");
        }

        public void Bind(TabViewModel model)
        {
            _model = model;

            // Подписываемся на изменения в списке команд через один Observable
            Observable.Merge(
                _model.Teams.ObserveCountChanged().Select(_ => Unit.Default),
                _model.Teams.ObserveAdd().Select(_ => Unit.Default),
                _model.Teams.ObserveRemove().Select(_ => Unit.Default),
                _model.Teams.ObserveReplace().Select(_ => Unit.Default)
            ).Subscribe(_ => RefreshTeamsList()).AddTo(_disposables);

            // Подписываемся на изменения игроков для перерасчета высоты
            _model.PlayerTeamChanged.Subscribe(_ => UpdateTeamHeights()).AddTo(_disposables);

            // Инициализация списка команд
            RefreshTeamsList();
            
            // Добавляем слушатель события изменения размера контейнера
            _teamsContainer.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            // Пересчитываем высоты при изменении размера контейнера
            UpdateTeamHeights();
        }

        private void RefreshTeamsList()
        {
            _teamsContainer.Clear();
            _teamElements.Clear();

            foreach (var team in _model.Teams)
            {
                var teamElement = _teamBlockTemplate.Instantiate();
                _teamsContainer.Add(teamElement);
                
                int teamId = team.TeamID.Value;
                _teamElements[teamId] = teamElement;
                
                SetupTeamElement(teamElement, team);
            }

            UpdateTeamHeights();
        }

        private void SetupTeamElement(VisualElement teamElement, TeamViewModel team)
        {
            // UI элементы
            var teamNameLabel = teamElement.Q<Label>("team-name");
            var playersCountLabel = teamElement.Q<Label>("players-count");
            var teamScoreLabel = teamElement.Q<Label>("team-score");
            var playerListView = teamElement.Q<ListView>("player-list-view");

            // Настройка ListView игроков
            playerListView.makeItem = () => _playerSlotTemplate.Instantiate();
            playerListView.bindItem = (playerElement, playerIndex) => 
            {
                // Используем сохраненный отсортированный список для привязки
                var sortedPlayers = CreateSortedPlayersList(team);
                if (playerIndex < sortedPlayers.Count)
                {
                    BindPlayerItem(playerElement, sortedPlayers[playerIndex]);
                }
            };

            // Настройки ListView
            playerListView.selectionType = SelectionType.None;
            playerListView.reorderable = false;
            playerListView.horizontalScrollingEnabled = false;
            playerListView.style.overflow = Overflow.Visible;
            
            // Привязываем информацию о команде
            BindTeamInfo(team, teamNameLabel, playersCountLabel, teamScoreLabel);

            // Обновляем список игроков
            var orderedPlayers = CreateSortedPlayersList(team);
            playerListView.itemsSource = orderedPlayers;
            
            // Подписываемся на изменения в команде
            team.TeamPlayersChanged
                .Subscribe(_ => {
                    var updatedPlayers = CreateSortedPlayersList(team);
                    playerListView.itemsSource = updatedPlayers;
        
                    // Скрывать ListView если список пуст
                    playerListView.style.display = updatedPlayers.Count > 0 ? DisplayStyle.Flex : DisplayStyle.None;
        
                    playerListView.Rebuild();
                    UpdateTeamHeights();
                })
                .AddTo(_disposables);
        }

        private void UpdateTeamHeights()
        {
            // Откладываем выполнение на следующий кадр, чтобы UI успел обновиться
            _teamsContainer.schedule.Execute(() => {
                float totalAvailableHeight = _teamsContainer.resolvedStyle.height;
                
                if (totalAvailableHeight <= 0)
                    return; // Контейнер еще не инициализирован
                
                // Используем внешний калькулятор для расчета высот
                var teamHeights = _heightCalculator.CalculateTeamHeights(
                    _model.Teams, 
                    _teamElements,
                    totalAvailableHeight);
                
                // Применяем рассчитанные высоты
                foreach (var pair in teamHeights)
                {
                    if (_teamElements.TryGetValue(pair.Key, out var element))
                    {
                        element.style.height = pair.Value;
                        element.style.minHeight = pair.Value;
                        element.style.maxHeight = pair.Value;
                        
                        // Обновляем ListView внутри команды
                        var playerListView = element.Q<ListView>("player-list-view");
                        if (playerListView != null)
                        {
                            // Высота ListView = высота команды - высота заголовка и другие элементы
                            var teamHeader = element.Q<VisualElement>("team-header");
                            var playersHeader = element.Q<VisualElement>("players-header");
                            
                            float headerHeight = teamHeader?.resolvedStyle.height ?? 35f;
                            float playersHeaderHeight = playersHeader?.resolvedStyle.height ?? 20f;
                            float listViewHeight = pair.Value - headerHeight - playersHeaderHeight - 5f;
                            
                            playerListView.style.height = listViewHeight;
                            playerListView.style.minHeight = listViewHeight;
                        }
                    }
                }
            });
        }

        private List<PlayerViewModel> CreateSortedPlayersList(TeamViewModel team)
        {
            // Создаем копию списка, отсортированную по рангу
            return team.Players.OrderBy(p => p.TeamRang.Value).ToList();
        }

        private void BindTeamInfo(TeamViewModel team, Label nameLabel, Label countLabel, Label scoreLabel)
        {
            // Привязка названия команды
            team.TeamName
                .Subscribe(name => nameLabel.text = name)
                .AddTo(_disposables);
    
            // Привязка цвета к названию команды
            team.TeamColor
                .Subscribe(color => {
                    nameLabel.style.color = color;
                    scoreLabel.style.color = color;
                })
                .AddTo(_disposables);

            // Привязка счета команды (текст)
            team.TotalScore
                .Subscribe(score => scoreLabel.text = score.ToString())
                .AddTo(_disposables);

            // Счетчик игроков
            Observable.CombineLatest(
                    team.TeamPlayersChanged.Select(_ => team.Players.Count),
                    team.MaxCount,
                    (count, max) => $"{count}/{max}")
                .Subscribe(text => countLabel.text = text)
                .AddTo(_disposables);
        }

        private void BindPlayerItem(VisualElement element, PlayerViewModel player)
        {
            var item = element.Q("player-list-item");
            var statusElement = element.Q<VisualElement>("player-status");
            var rankLabel = element.Q<Label>("rank");
            var nameLabel = element.Q<Label>("name");
            var killsLabel = element.Q<Label>("kill-stats");
            var deathsLabel = element.Q<Label>("death-stats");
            var pingLabel = element.Q<Label>("ping-stats");
            var scoreLabel = element.Q<Label>("total-score");

            // Биндим все свойства игрока
            player.IsDead.Subscribe(isDead =>
            {
                statusElement.style.visibility = isDead ? Visibility.Visible : Visibility.Hidden;
            })
            .AddTo(_disposables);
            
            player.TeamRang
                .Subscribe(rank => rankLabel.text = rank.ToString())
                .AddTo(_disposables);

            player.Name
                .Subscribe(name => nameLabel.text = name)
                .AddTo(_disposables);

            player.Kills
                .Subscribe(kills => killsLabel.text = kills.ToString())
                .AddTo(_disposables);

            player.Deaths
                .Subscribe(deaths => deathsLabel.text = deaths.ToString())
                .AddTo(_disposables);

            player.Ping
                .Subscribe(ping => pingLabel.text = ping.ToString())
                .AddTo(_disposables);

            player.TotalScore
                .Subscribe(score => scoreLabel.text = score.ToString())
                .AddTo(_disposables);

            // Применяем стили для локального игрока
            player.IsLocal
                .Subscribe(isLocal =>
                {
                    if (isLocal)
                    {
                        item.AddToClassList("local-player");
                        item.RemoveFromClassList("remote-player");
                    }
                    else
                    {
                        item.AddToClassList("remote-player");
                        item.RemoveFromClassList("local-player");
                    }
                })
                .AddTo(_disposables);
        }

        public void Unbind()
        {
            if (_teamsContainer != null)
            {
                _teamsContainer.UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            }
            
            _teamElements.Clear();
            _model = null;
            _disposables.Clear();
        }
    }
}