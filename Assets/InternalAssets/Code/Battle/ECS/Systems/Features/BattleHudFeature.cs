using ProjectOlog.Code.UI.HUD.ChatPanel;
using ProjectOlog.Code.UI.HUD.Debugger.Systems;
using ProjectOlog.Code.UI.HUD.InventoryPanel;
using ProjectOlog.Code.UI.HUD.KillPanel.Systems;
using ProjectOlog.Code.UI.HUD.KillPanel.Systems.PlayerNotifications;
using ProjectOlog.Code.UI.HUD.PlayerStatus.HealthPanel;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Systems;
using ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Systems.PlayerNotifications;
using ProjectOlog.Code.UI.HUD.Systems;
using ProjectOlog.Code.UI.HUD.Tab;
using Scellecs.Morpeh;

namespace ProjectOlog.Code.Battle.ECS.Systems.Features
{
    public sealed class BattleHudFeature: FeatureSystemsBlock
    {
        public override void Execute(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<HealthLogicSystem>(_systemsGroup);
            
            _systemsFactory.CreateSystem<ChatLogicSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<TabLogicSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<InventoryLogicSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<DebuggerLogicSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<NotificationLogicSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<KillbarLogicSystem>(_systemsGroup);

            _systemsFactory.CreateSystem<VisibleHudCameraSystem>(_systemsGroup);

            ExecuteAdditionalSystems(_systemsGroup, _systemsFactory);
        }

        private void ExecuteAdditionalSystems(SystemsGroup _systemsGroup, EcsSystemsFactory _systemsFactory)
        {
            _systemsFactory.CreateSystem<KillbarPlayerEnvironmentSystem>(_systemsGroup);
            _systemsFactory.CreateSystem<NotificationPlayerRespawnSystem>(_systemsGroup);
        }
    }
}