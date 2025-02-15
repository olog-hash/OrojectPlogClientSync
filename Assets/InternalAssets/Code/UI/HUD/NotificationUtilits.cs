using System;
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
        
        public static void SendChatMessageNone(string messageText)
        {
            var message = new ChatMessageData(ChatMessageType.None, "None", messageText);

            SendChatMessageEvent(message);
        }

        public static void SendChatMessageAlert(string messageText)
        {
            var message = new ChatMessageData(ChatMessageType.Alert, "None", messageText);

            SendChatMessageEvent(message);
        }

        public static void SendChatMessageSystem(string messageText)
        {
            var message = new ChatMessageData(ChatMessageType.System, "None", messageText);

            SendChatMessageEvent(message);
        }

        public static void SendChatMessageUser(string from, string messageText)
        {
            var message = new ChatMessageData(ChatMessageType.User, from, messageText);

            SendChatMessageEvent(message);
        }

        public static void SendChatMessagePlayer(string from, string messageText, byte Team = 0)
        {
            var message = new ChatMessageData(ChatMessageType.Player, from, messageText);

            SendChatMessageEvent(message);
        }


        public static void SendChatMessageEvent(ChatMessageData chatMessageData)
        {
            OnChatMessageReceived?.Invoke(chatMessageData);
        }
    }
}
