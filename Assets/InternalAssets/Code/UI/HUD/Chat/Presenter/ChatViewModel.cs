using System;
using ObservableCollections;
using ProjectOlog.Code.Infrastructure.Application.Layers;
using ProjectOlog.Code.Network.Infrastructure.NetWorkers.Users;
using ProjectOlog.Code.UI.Core;
using ProjectOlog.Code.UI.HUD.Chat.Models;
using R3;

namespace ProjectOlog.Code.UI.HUD.Chat.Presenter
{
    public class ChatViewModel : BaseViewModel, ILayer
    {
        public const int MaxMessageLength = 170;
        private const int MaxVisibleMessages = 20;
        private const float DefaultMessageLifetime = 5f;
        
        public ReactiveProperty<bool> IsInputActive { get; } = new ReactiveProperty<bool>(false);
        public ObservableList<ChatMessageModel> Messages { get; } = new ObservableList<ChatMessageModel>();
        public ReactiveProperty<string> CurrentInputText { get; } = new ReactiveProperty<string>("");

        // Инструменты
        private UserChatNetworker _userChatNetworker;

        public ChatViewModel(UserChatNetworker userChatNetworker) : base()
        {
            _userChatNetworker = userChatNetworker;

            NotificationUtilits.OnChatMessageReceived += AddMessage;
        }
        
        public void ShowLayer()
        {
            IsInputActive.Value = true;
        }

        public void HideLayer()
        {
            IsInputActive.Value = false;
            
            TrySendMessage(CurrentInputText.Value);
        }
        
        public void TrySendMessage(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                if (text.Length > MaxMessageLength)
                {
                    text = text.Substring(0, MaxMessageLength);
                }
                
                _userChatNetworker.SendMessageRequest(text);
            }
            
            CurrentInputText.Value = "";
        }
        
        private void AddMessage(ChatMessageModel chatMessage)
        {
            Messages.Add(chatMessage);
            
            if (Messages.Count > MaxVisibleMessages)
            {
                var oldestMessage = Messages[0];
                Messages.RemoveAt(0);
                oldestMessage.Dispose();
            }
        }
        
        public void OnUpdate(float deltaTime)
        {
            // Обновляем время жизни всех сообщений
            foreach (var message in Messages)
            {
                message.UpdateLifetime(deltaTime);
            }
        }
        
        public override void OnHide()
        {
            IsInputActive.Value = false;
            CurrentInputText.Value = "";
        }
        
        public override void Dispose()
        {
            base.Dispose();

            foreach (var message in Messages)
            {
                message.Dispose();
            }
            
            Messages.Clear();
            IsInputActive.Dispose();
            CurrentInputText.Dispose();
            
            NotificationUtilits.OnChatMessageReceived -= AddMessage;
        }
    }
}