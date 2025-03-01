using System;
using System.Collections.Generic;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Network.Infrastructure.NetWorkers.Users;
using ProjectOlog.Code.UI.Core;

namespace ProjectOlog.Code.UI.HUD.ChatPanel
{
    public class ChatViewModel: BaseViewModel, ILayer
    {
        public const int MAX_MESSAGE_LENGTH = 170;
        public const int MAX_COUNT = 14;

        public Action<bool> OnShowHideChanged;
        public Action OnMessageReceived;
        public Action OnMessageVisibilityChanged;
        
        public readonly List<ChatMessageData> ChatMessages;

        private UserChatNetworker _userChatNetworker;
        

        public ChatViewModel(UserChatNetworker userChatNetworker)
        {
            _userChatNetworker = userChatNetworker;

            OnShowHideChanged = null;
            OnMessageReceived = null;
            OnMessageVisibilityChanged = null;
            ChatMessages = new List<ChatMessageData>();

            NotificationUtilits.OnChatMessageReceived += AddMessage;
        }

        public void AddMessage(ChatMessageData chatMessage)
        {
            ChatMessages.Add(chatMessage);
            
            if (ChatMessages.Count > MAX_COUNT)
            {
                ChatMessages.RemoveAt(0);
            }
            
            OnMessageReceived?.Invoke();
        }

        public void ClearMessages()
        {
            ChatMessages.Clear();
            OnMessageReceived?.Invoke();
        }

        public void SendMessage(string sendMessageText)
        {
            if (!string.IsNullOrWhiteSpace(sendMessageText))
            {
                if (sendMessageText.Length > MAX_MESSAGE_LENGTH)
                {
                    sendMessageText = sendMessageText.Substring(0, MAX_MESSAGE_LENGTH);
                }
                
                _userChatNetworker.SendMessageRequest(sendMessageText);
            }
        }

        public void OnUpdate(float deltaTime)
        {
            MessagesLifetimeUpdate(deltaTime);
        }

        private void MessagesLifetimeUpdate(float deltaTime)
        {
            bool visibilityChanged = false;
            foreach (var message in ChatMessages)
            {
                bool prevVisibility = message.IsVisible;
                message.OnUpdate(deltaTime);
                if (prevVisibility != message.IsVisible)
                    visibilityChanged = true;
            }

            if (visibilityChanged)
            {
                OnMessageVisibilityChanged?.Invoke();
            }
        }

        public void OnShow()
        {
            OnShowHideChanged?.Invoke(true);
        }

        public void OnHide()
        {
            OnShowHideChanged?.Invoke(false);
        }
    }
}