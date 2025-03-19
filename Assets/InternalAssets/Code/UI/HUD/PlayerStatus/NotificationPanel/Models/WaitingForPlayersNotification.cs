using ProjectOlog.Code.Network.Profiles.Users;

namespace ProjectOlog.Code.UI.HUD.PlayerStatus.NotificationPanel.Notifications
{
    public class WaitingForPlayersNotification : AbstractNotification
    {
        private readonly NetworkUsersContainer _usersContainer;
        private readonly int _requiredPlayerCount;

        public WaitingForPlayersNotification(NetworkUsersContainer usersContainer, int requiredPlayerCount)
        {
            _usersContainer = usersContainer;
            _requiredPlayerCount = requiredPlayerCount;
        }

        public override void Initialize()
        {
            _usersContainer.OnUsersUpdate += UpdateNotificationMessage;
            UpdateNotificationMessage();
        }

        public override void OnUpdate(float deltaTime)
        {
            if (_usersContainer.UsersCount >= _requiredPlayerCount)
            {
                Close();
            }
        }

        private void UpdateNotificationMessage()
        {
            NotificationMessage.Value = $"Waiting for players: {_usersContainer.UsersCount}/{_requiredPlayerCount}";
        }

        public override void Close()
        {
            _usersContainer.OnUsersUpdate -= UpdateNotificationMessage;
            base.Close();
        }
    }
}