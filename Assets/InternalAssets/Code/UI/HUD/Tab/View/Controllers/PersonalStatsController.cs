using System;
using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using R3;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.Tab.View.Controllers
{
    // Контроллер для персональной статистики
    public class PersonalStatsController : UIToolkitElementView
    {
        private Label _levelLabel;
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
            _experienceLabel = Root.Q<Label>("global-experience");
            _experienceGainedLabel = Root.Q<Label>("experience-gained");
            _experienceGainedBottomLabel = Root.Q<Label>("experience-gained-bottom");
            _experienceToNextLevelLabel = Root.Q<Label>("experience-left");
        }
    
        public void Bind(TabViewModel model)
        {
            _model = model;

            // Подписываемся на изменения уровня
            _model.PlayerLevel
                .Subscribe(level => _levelLabel.text = level.ToString())
                .AddTo(_disposables);
    
            // Вычисляем актуальный опыт (базовый + полученный)
            Observable.CombineLatest(
                    _model.PlayerCurrentExperience,
                    _model.PlayerExperienceGained,
                    (current, gained) => current + gained)
                .Subscribe(total => _actualExperience.Value = total)
                .AddTo(_disposables);
    
            // Отображаем актуальный опыт / опыт для следующего уровня
            Observable.CombineLatest(
                    _actualExperience, 
                    _model.PlayerNextLevelExperience, 
                    (actual, next) => $"{actual}/{next}")
                .Subscribe(text => _experienceLabel.text = text)
                .AddTo(_disposables);
    
            // Отображаем полученный опыт
            _model.PlayerExperienceGained
                .Subscribe(exp => _experienceGainedLabel.text = exp.ToString())
                .AddTo(_disposables);
            
            _model.PlayerExperienceGained
                .Subscribe(exp => _experienceGainedBottomLabel.text = exp.ToString())
                .AddTo(_disposables);
    
            // Вычисляем и отображаем опыт до следующего уровня
            Observable.CombineLatest(
                    _actualExperience,
                    _model.PlayerNextLevelExperience,
                    (actual, next) => Math.Max(0, next - actual))
                .Subscribe(remaining => _experienceToNextLevelLabel.text = remaining.ToString())
                .AddTo(_disposables);
        }
    
        public void Unbind()
        {
            _disposables.Clear();
            _actualExperience.Dispose();
            _model = null;
        }
    }
}