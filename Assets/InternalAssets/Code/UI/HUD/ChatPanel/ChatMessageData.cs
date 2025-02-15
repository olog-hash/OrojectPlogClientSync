namespace ProjectOlog.Code.UI.HUD.ChatPanel
{
    public class ChatMessageData
    {
        public bool IsVisible { get; private set; } = true;
        public float LifeTime { get; private set; }
        public ChatMessageType Type = ChatMessageType.None;
        public string FromName = "NONE";
        public string Text = string.Empty;

        public ChatMessageData(ChatMessageType type, string text, int lifeTime = 5)
        {
            Type = type;
            Text = text;
            LifeTime = lifeTime;

            IsVisible = true;
        }

        public ChatMessageData(ChatMessageType type, string from, string text, int lifeTime = 5)
        {
            Type = type;
            FromName = from;
            Text = text;
            LifeTime = lifeTime;
            
            IsVisible = true;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsVisible) return;
            
            LifeTime -= deltaTime;
            if (LifeTime <= 0)
            {
                LifeTime = 0;
                IsVisible = false;
            }
        }
    }

    public enum ChatMessageType
    {
        None,
        Alert,
        System,
        User,
        Player,
    }
}