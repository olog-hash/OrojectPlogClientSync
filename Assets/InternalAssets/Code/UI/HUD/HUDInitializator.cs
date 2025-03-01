using ProjectOlog.Code.Engine.Cameras.Core;
using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.ChatPanel;
using ProjectOlog.Code.UI.HUD.ChatPanel.DefaultChatView;
using ProjectOlog.Code.UI.HUD.CrossHair.CrossPanel;
using ProjectOlog.Code.UI.HUD.CrossHair.InteractionPanel;
using ProjectOlog.Code.UI.HUD.Debugger;
using ProjectOlog.Code.UI.HUD.InventoryPanel;
using ProjectOlog.Code.UI.HUD.KillPanel;
using ProjectOlog.Code.UI.HUD.KillPanel.View;
using ProjectOlog.Code.UI.HUD.PlayerStatus;
using ProjectOlog.Code.UI.HUD.PlayerStatus.HealthPanel;
using ProjectOlog.Code.UI.HUD.PlayerStatus.HealthPanel.DefaultHealthView;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel;
using ProjectOlog.Code.UI.HUD.TabPanel;
using ProjectOlog.Code.UI.HUD.TabPanel.DefaultTabView;
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


        [Inject]
        public void Construct(HUDFactory hudFactory, InterfaceBindLogic interfaceBindLogic)
        {
            _hudFactory = hudFactory;
            _interfaceBindLogic = interfaceBindLogic;

            Initialize();
        }

        private void Initialize()
        {
            ResetServices();
            RegisterViewModels();
            RegisterViews();
            BindInterfaces();
            BindCameras();
        }

        private void ResetServices()
        {
            NotificationUtilits.Reset();
        }

        private void RegisterViewModels()
        {
            _interfaceBindLogic.RegisterViewModel(_hudFactory.CreateViewModel<ChatViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.CreateViewModel<TabViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.CreateViewModel<InventoryViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.CreateViewModel<InteractionViewModel>());
            
            _interfaceBindLogic.RegisterViewModel(_hudFactory.CreateViewModel<HealthViewModel>());
            
            _interfaceBindLogic.RegisterViewModel(_hudFactory.CreateViewModel<DebuggerViewModel>());
            
            _interfaceBindLogic.RegisterViewModel(_hudFactory.CreateViewModel<NotificationViewModel>());
            _interfaceBindLogic.RegisterViewModel(_hudFactory.CreateViewModel<KillBarViewModel>());
            
            _interfaceBindLogic.RegisterViewModel(_hudFactory.CreateViewModel<PlayerBlockViewModel>());
            
            _interfaceBindLogic.RegisterViewModel(_hudFactory.CreateViewModel<CrossViewModel>());
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
            _interfaceBindLogic.SwitchView<DefaultChatView>();
            _interfaceBindLogic.SwitchView<DefaultHealthView>();
            _interfaceBindLogic.SwitchView<DefaultTabView>();
            _interfaceBindLogic.SwitchView<InteractionView>();
            _interfaceBindLogic.SwitchView<CrossView>();
            
            _interfaceBindLogic.SwitchView<DebuggerView>();
            _interfaceBindLogic.SwitchView<DefaultNotificationView>();
            _interfaceBindLogic.SwitchView<KillBarView>();
            
            _interfaceBindLogic.SwitchView<InventoryView>();
        }
        
        private void BindCameras()
        {
            MainCamera.Instance.InitializeCameras(_hudMainCamera, _hudCrosshairCamera);
        }
        
        public void OnDestroy()
        {
            
        }
    }
}