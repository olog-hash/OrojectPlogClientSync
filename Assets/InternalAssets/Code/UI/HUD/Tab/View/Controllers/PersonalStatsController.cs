using System;
using System.Linq;
using ObservableCollections;
using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.HUD.Tab.Models;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.Tab.View.Controllers
{
    // Контроллер для персональной статистики
    public class PersonalStatsController : UIToolkitElementView
    {
        private Label _levelLabel;
        private Label _localRatingLabel;
        private Label _experienceLabel;
        private Label _experienceGainedLabel;
        private Label _experienceGainedBottomLabel;
        private Label _experienceToNextLevelLabel;
    
        private TabViewModel _model;
        
        // Реактивное свойство для вычисления актуального опыта (базовый + полученный)
        private ReactiveProperty<int> _actualExperience = new ReactiveProperty<int>();

        public PersonalStatsController(VisualElement root) : base(root)
        {
            
        }
        
        protected override void SetVisualElements()
        {
            _levelLabel = Root.Q<Label>("level");
            _localRatingLabel = Root.Q<Label>("local-rating");
            _experienceLabel = Root.Q<Label>("global-experience");
            _experienceGainedLabel = Root.Q<Label>("experience-gained");
            _experienceGainedBottomLabel = Root.Q<Label>("experience-gained-bottom");
            _experienceToNextLevelLabel = Root.Q<Label>("experience-left");
        }
    
        public void Bind(TabViewModel model)
        {
            _model = model;

            // Подписываемся на изменения уровня
            _model.PlayerStatsModel.PlayerLevel
                .Subscribe(level => _levelLabel.text = level.ToString())
                .AddTo(_disposables);
    
            // Вычисляем актуальный опыт (базовый + полученный)
            Observable.CombineLatest(
                    _model.PlayerStatsModel.PlayerCurrentExperience,
                    _model.PlayerStatsModel.PlayerExperienceGained,
                    (current, gained) => current + gained)
                .Subscribe(total => _actualExperience.Value = total)
                .AddTo(_disposables);
    
            // Отображаем актуальный опыт / опыт для следующего уровня
            Observable.CombineLatest(
                    _actualExperience, 
                    _model.PlayerStatsModel.PlayerNextLevelExperience, 
                    (actual, next) => $"{actual}/{next}")
                .Subscribe(text => _experienceLabel.text = text)
                .AddTo(_disposables);
    
            // Отображаем полученный опыт
            _model.PlayerStatsModel.PlayerExperienceGained
                .Subscribe(exp => _experienceGainedLabel.text = exp.ToString())
                .AddTo(_disposables);
            
            _model.PlayerStatsModel.PlayerExperienceGained
                .Subscribe(exp => _experienceGainedBottomLabel.text = exp.ToString())
                .AddTo(_disposables);
    
            // Вычисляем и отображаем опыт до следующего уровня
            Observable.CombineLatest(
                    _actualExperience,
                    _model.PlayerStatsModel.PlayerNextLevelExperience,
                    (actual, next) => Math.Max(0, next - actual))
                .Subscribe(remaining => _experienceToNextLevelLabel.text = remaining.ToString())
                .AddTo(_disposables);
            
            // НОВЫЙ КОД: Подписка на события изменения в командах
            // Функция для подписки на изменения в команде
            Action<TeamModel> subscribeToTeam = (team) => {
                team.TeamPlayersChanged
                    .Subscribe(_ => UpdateLocalPlayerRating())
                    .AddTo(_disposables);
            };

            // Подписываемся на изменения во всех существующих командах
            foreach (var team in _model.TeamsManagerModel.Teams)
            {
                subscribeToTeam(team);
            }

            // Подписываемся на добавление новых команд
            _model.TeamsManagerModel.Teams.ObserveAdd()
                .Subscribe(e => subscribeToTeam(e.Value))
                .AddTo(_disposables);

            // Подписываемся на изменения в игроках и их командах
            _model.TeamsManagerModel.PlayerTeamChanged
                .Subscribe(_ => UpdateLocalPlayerRating())
                .AddTo(_disposables);

            // Инициализируем начальное значение
            UpdateLocalPlayerRating();
        }
        
        // НОВЫЙ МЕТОД: Обновление рейтинга локального игрока
        private void UpdateLocalPlayerRating()
        {
            // Находим всех локальных игроков
            var localPlayers = _model.TeamsManagerModel.Players
                .Where(p => p.IsLocal.Value)
                .ToList();

            // Если нет или несколько локальных игроков - текст пустой
            if (localPlayers.Count != 1)
            {
                _localRatingLabel.text = "";
                return;
            }

            // Получаем единственного локального игрока
            var localPlayer = localPlayers[0];
            
            // Находим команду игрока
            var playerTeam = _model.TeamsManagerModel.Teams
                .FirstOrDefault(t => t.TeamID.Value == localPlayer.TeamID.Value);

            // Если команды нет - текст пустой
            if (playerTeam == null)
            {
                _localRatingLabel.text = "";
                return;
            }

            // Количество игроков в команде
            int totalPlayers = playerTeam.Players.Count;
            
            // Место локального игрока (TeamRang начинается с 1)
            int playerRank = localPlayer.TeamRang.Value;

            // Обновляем текст в формате "X/Y"
            _localRatingLabel.text = $"{playerRank}/{totalPlayers}";
        }
    
        public void Unbind()
        {
            _disposables.Clear();
            _actualExperience.Dispose();
            _model = null;
        }
    }
}