using ProjectOlog.Code._InDevs.Data;
using ProjectOlog.Code.Battle.ECS.Rules.ComplexRules.SpawnPlayerRequestRule;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Notifications
{
    public class DefaultSpawnNotification : AbstractNotification
    {
        private LocalPlayerMonitoring _localPlayerMonitoring;
        private float _remainingTime;

        public DefaultSpawnNotification(LocalPlayerMonitoring localPlayerMonitoring)
        {
            _localPlayerMonitoring = localPlayerMonitoring;
        }

        public override void Initialize()
        {
            _remainingTime = SpawnRequestRule.MaxCooldown; 
            UpdateNotificationMessage();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!_localPlayerMonitoring.IsDead())
            {
                Close();
                return;
            }

            _remainingTime -= deltaTime;
            if (_remainingTime <= 0f)
            {
                NotificationMessage = "Press 'Fire' to respawn";
            }
            else
            {
                UpdateNotificationMessage();
            }
        }

        private void UpdateNotificationMessage()
        {
            NotificationMessage = $"Respawning in {_remainingTime:F0} seconds";
        }
        
        // Должен встречать с сообщением "Возрождение через n секунд" и каждую секунду менять показатель до нуля.
        // После того как станет нулевым - менять запись "Для возрождения нажмите огонь"
    }
}