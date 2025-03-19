using System.Collections.Generic;
using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using ProjectOlog.Code.UI.HUD.Tab.View.Controllers;
using R3;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.Tab.View
{
    public class TabView : UIToolkitScreen<TabViewModel>
    {
        [SerializeField] private VisualTreeAsset _teamBlockTemplate;
        [SerializeField] private VisualTreeAsset _playerSlotTemplate;

        // Контроллеры для различных блоков UI
        private TeamsBlockController _teamsController;
        private PersonalStatsController _personalStatsController;
        private HeaderController _headerController;
        
        protected override void SetVisualElements()
        {
            // Проверка, что ListView найден
            var teamListView = _root.Q<ListView>("team-list-view");
            if (teamListView == null)
            {
                Debug.LogError("team-list-view not found in UI Document!");
                return;
            }
        }
        
        protected override void OnBind(TabViewModel model)
        {
            HideOnAwake = true;
            
            // Создаем контроллеры и инициализируем их
            _headerController = new HeaderController(_root);
            _personalStatsController = new PersonalStatsController(_root);
            _teamsController = new TeamsBlockController(_root, _teamBlockTemplate, _playerSlotTemplate);
    
            // Привязываем контроллеры к модели
            _headerController.Bind(model);
            _personalStatsController.Bind(model);
            _teamsController.Bind(model);
        }

        protected override void OnUnbind(TabViewModel model)
        {
            // Отвязываем контроллеры
            _headerController?.Unbind();
            _personalStatsController?.Unbind();
            _teamsController?.Unbind();
        }
    }
}