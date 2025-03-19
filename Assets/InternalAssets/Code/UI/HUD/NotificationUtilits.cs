using System;
using ProjectOlog.Code.Network.Gameplay.Core.Enums;
using ProjectOlog.Code.UI.HUD.Chat;
using ProjectOlog.Code.UI.HUD.Chat.Models;
using ProjectOlog.Code.UI.HUD.ChatPanel;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD
{
    public static class NotificationUtilits
    {
        public static Action<ChatMessageModel> OnChatMessageReceived;

        public static void Reset()
        {
            OnChatMessageReceived = null;
        }

        public static void ProcessServerMessage(ENetworkChatMessageType messageType, string messageText)
        {
            var messageModel = MessageBuilder.SystemMessage(messageText);
            
            SendChatMessageEvent(messageModel);
        }

        public static void ProcessPlayerMessage(string fromUserName, string messageText)
        {
            var messageModel = MessageBuilder.PlayerMessage(fromUserName, messageText);
            
            SendChatMessageEvent(messageModel);
        }
        
        public static void ProcessNoneMessage(string messageText)
        {
            var messageBuilder = new MessageBuilder();
            var messageModel = messageBuilder.AddGrayText(messageText).Build();
            
            SendChatMessageEvent(messageModel);
        }

        public static void ProcessAlertMessage(string messageText)
        {
            var messageModel = MessageBuilder.ErrorMessage(messageText);
            
            SendChatMessageEvent(messageModel);
        }

        public static void ProcessSystemMessage(string messageText)
        {
            var messageModel = MessageBuilder.SystemMessage(messageText);
            
            SendChatMessageEvent(messageModel);
        }

        public static void SendChatMessageEvent(ChatMessageModel chatMessageModel)
        {
            OnChatMessageReceived?.Invoke(chatMessageModel);
        }
    }
}
