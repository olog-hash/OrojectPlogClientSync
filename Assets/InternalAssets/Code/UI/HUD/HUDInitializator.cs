using System;
using ProjectOlog.Code.Engine.Cameras.Core;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.Core.Services;
using ProjectOlog.Code.UI.HUD.Chat.Presenter;
using ProjectOlog.Code.UI.HUD.Chat.View;
using ProjectOlog.Code.UI.HUD.ChatPanel;
using ProjectOlog.Code.UI.HUD.CrossHair.CrossPanel;
using ProjectOlog.Code.UI.HUD.CrossHair.InteractionPanel;
using ProjectOlog.Code.UI.HUD.Debugger;
using ProjectOlog.Code.UI.HUD.InventoryPanel;
using ProjectOlog.Code.UI.HUD.Killbar.Presenter;
using ProjectOlog.Code.UI.HUD.Killbar.View;
using ProjectOlog.Code.UI.HUD.KillPanel;
using ProjectOlog.Code.UI.HUD.MenuESC.Presenter;
using ProjectOlog.Code.UI.HUD.MenuESC.View;
using ProjectOlog.Code.UI.HUD.Overlays.DamageScreen.Presenter;
using ProjectOlog.Code.UI.HUD.Overlays.DamageScreen.View;
using ProjectOlog.Code.UI.HUD.PlayerStats.Presenter;
using ProjectOlog.Code.UI.HUD.PlayerStats.View;
using ProjectOlog.Code.UI.HUD.PlayerStatus;
using ProjectOlog.Code.UI.HUD.PlayerStatus.HealthPanel;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Presenter;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.View;
using ProjectOlog.Code.UI.HUD.Tab.Presenter;
using ProjectOlog.Code.UI.HUD.Tab.View;
using ProjectOlog.Code.UI.Shared.Settings.Presenter;
using ProjectOlog.Code.UI.Shared.Settings.View;
using UnityEngine;
using Zenject;

namespace ProjectOlog.Code.UI.HUD
{
    public class HUDInitializator : MonoBehaviour
    {
        [SerializeField] private Camera _hudMainCamera;
        [SerializeField] private Camera _hudCrosshairCamera;
        
        // Tools
        private HUDFactory _hudFactory;
        private InterfaceBindLogic _interfaceBindLogic;
        private LayersManager _layersManager;
        private LayersViewModelContainer _layersViewModelContainer;

        [Inject]
        public void Construct(HUDFactory hudFactory, InterfaceBindLogic interfaceBindLogic, LayersManager layersManager)
        {
            _hudFactory = hudFactory;
            _interfaceBindLogic = interfaceBindLogic;
            _layersManager = layersManager;

            _layersViewModelContainer = new LayersViewModelContainer(_layersManager);
        }

        private void Awake()
        {
            ResetServices();
            RegisterViewModels();
            RegisterLayers();
            RegisterViews();
            BindInterfaces();
            BindCameras();
        }

        private void ResetServices()
        {
            NotificationUtilits.Reset();
            GlobalUIVisibility.Reset();
        }

        private void RegisterViewModels()
        {
            // Системные интерфейсы (меню паузы, настройки)
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<MenuEscViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<SettingsViewModel>());
    
            // Основные интерактивные панели
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<ChatViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<TabViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<InventoryViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<InteractionViewModel>());
    
            // Основные элементы HUD
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<PlayerBlockViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<PlayerStatsViewModel>());    
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<CrossViewModel>());
    
            // Информационные и вспомогательные элементы
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<NotificationViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<KillbarViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<DebuggerViewModel>());
    
            // Визуальные эффекты
            _interfaceBindLogic.RegisterViewModel(_hudFactory.ResolveViewModel<DamageScreenViewModel>());
        }

        private void RegisterLayers()
        {
            // Регистрируем все интерфейсы которые являются слоями (как следствие интерактивными)
            RegisterLayerConfig<ChatViewModel>(ELayerChannel.Channel_1, LayerInputMode.SelectedPanel);
            RegisterLayerConfig<InventoryViewModel>(ELayerChannel.Channel_1, LayerInputMode.Freedom);
            RegisterLayerConfig<TabViewModel>(ELayerChannel.Channel_1, LayerInputMode.Game);
            RegisterLayerConfig<MenuEscViewModel>(ELayerChannel.Channel_2, LayerInputMode.SelectedPanel);
            RegisterLayerConfig<SettingsViewModel>(ELayerChannel.Channel_3, LayerInputMode.SelectedPanel);
        }

        private void RegisterViews()
        {
            // Получаем все компоненты BaseScreen в дочерних объектах
            BaseScreen[] screens = GetComponentsInChildren<BaseScreen>(true);

            // Распределяем компоненты по спискам
            foreach (var viewScreen in screens)
            {
                _interfaceBindLogic.RegisterScreen(viewScreen);
            }
        }

        // Тут происходит Бинд, а также активация тех интерфейсов, что были выбраны.
        private void BindInterfaces()
        {
            // Системные интерфейсы
            _interfaceBindLogic.SwitchView<MenuEscView>();
            _interfaceBindLogic.SwitchView<SettingsView>();
    
            // Основные интерактивные панели
            _interfaceBindLogic.SwitchView<ChatView>();
            _interfaceBindLogic.SwitchView<TabView>();
            _interfaceBindLogic.SwitchView<InventoryView>();
            _interfaceBindLogic.SwitchView<InteractionView>();
    
            // Основные элементы HUD
            _interfaceBindLogic.SwitchView<PlayerStatsView>();
            _interfaceBindLogic.SwitchView<CrossView>();
    
            // Информационные и вспомогательные элементы
            _interfaceBindLogic.SwitchView<DebuggerView>();
            _interfaceBindLogic.SwitchView<NotificationView>();
            _interfaceBindLogic.SwitchView<KillbarView>();
    
            // Визуальные эффекты
            _interfaceBindLogic.SwitchView<DamageScreenView>();
        }
        
        private void BindCameras()
        {
            MainCamera.Instance.InitializeCameras(_hudMainCamera, _hudCrosshairCamera);
        }
        
        public void OnDestroy()
        {
            _layersViewModelContainer.ClearLayers();
        }
        
        // Вспомогательный метод для регистрации слоя с типизированным ViewModel
        private void RegisterLayerConfig<T>(ELayerChannel channel, LayerInputMode inputMode) 
            where T : BaseViewModel, ILayer
        {
            var viewModel = _hudFactory.ResolveViewModel<T>();
            _layersViewModelContainer.RegisterLayer(viewModel, channel, inputMode);
        }
    }
}