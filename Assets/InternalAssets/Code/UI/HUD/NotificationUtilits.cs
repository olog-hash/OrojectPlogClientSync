using System;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using ProjectOlog.Code.UI.HUD.ChatPanel;

namespace ProjectOlog.Code.UI.HUD
{
    public static class NotificationUtilits
    {
        public static Action<ChatMessageData> OnChatMessageReceived;

        public static void Reset()
        {
            OnChatMessageReceived = null;
        }

        public static void ProcessServerMessage(ENetworkChatMessageType messageType, string messageText)
        {
            var message = new ChatMessageData(ChatMessageType.System, "None", messageText);

            SendChatMessageEvent(message);
        }

        public static void ProcessPlayerMessage(string fromUserName, string messageText)
        {
            var message = new ChatMessageData(ChatMessageType.Player, fromUserName, messageText);

            SendChatMessageEvent(message);
        }
        
        public static void ProcessNoneMessage(string messageText)
        {
            var message = new ChatMessageData(ChatMessageType.None, "None", messageText);

            SendChatMessageEvent(message);
        }

        public static void ProcessAlertMessage(string messageText)
        {
            var message = new ChatMessageData(ChatMessageType.Alert, "None", messageText);

            SendChatMessageEvent(message);
        }

        public static void ProcessSystemMessage(string messageText)
        {
            var message = new ChatMessageData(ChatMessageType.System, "None", messageText);

            SendChatMessageEvent(message);
        }

        public static void SendChatMessageEvent(ChatMessageData chatMessageData)
        {
            OnChatMessageReceived?.Invoke(chatMessageData);
        }
    }
}
