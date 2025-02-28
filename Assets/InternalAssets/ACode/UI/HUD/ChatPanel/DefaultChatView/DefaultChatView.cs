using System.Collections.Generic;
using ProjectOlog.Code.UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectOlog.Code.UI.HUD.ChatPanel.DefaultChatView
{
    public class DefaultChatView: AbstractScreen<ChatViewModel>
    {
        [Header("Main items")] 
        [SerializeField] private Transform _messagesRoot;
        [SerializeField] private MessageSlotView _messagePrefab;
        [SerializeField] private InputField _inputField;
        
        // Tools
        private List<MessageSlotView> _messageSlots = new List<MessageSlotView>();
        private bool _isActiveMode;
        
        // ViewModel
        private ChatViewModel _currentViewModel;

        protected override void OnBind(ChatViewModel model)
        {
            _currentViewModel = model;
            _currentViewModel.OnMessageReceived += OnMessageReceived;
            _currentViewModel.OnShowHideChanged += OnShowHideChanged;
            _currentViewModel.OnMessageVisibilityChanged += OnMessageVisibilityChanged;

            _messageSlots.Clear();
            for (int i = 0; i < ChatViewModel.MAX_COUNT; i++)
            {
                var slotMessage = Instantiate(_messagePrefab, _messagesRoot);
                slotMessage.gameObject.SetActive(true);
                
                slotMessage.Clear();
                _messageSlots.Add(slotMessage);
            }
        }

        protected override void OnUnbind(ChatViewModel model)
        {
            _currentViewModel.OnMessageReceived -= OnMessageReceived;
            _currentViewModel.OnShowHideChanged -= OnShowHideChanged;
            _currentViewModel.OnMessageVisibilityChanged -= OnMessageVisibilityChanged;
        }

        private void OnMessageReceived()
        {
            UpdateChatMessages(_isActiveMode);
        }

        private void OnMessageVisibilityChanged()
        {
            UpdateChatMessages(_isActiveMode);
        }

        private void UpdateChatMessages(bool showHideMessages = false)
        {
            // Очищаем все данные
            for (int i = 0; i < _messageSlots.Count; i++)
            {
                _messageSlots[i].Clear();
            }
            
            int limit = Mathf.Min(_currentViewModel.ChatMessages.Count, _messageSlots.Count);
            for (int i = 0; i < limit; i++)
            { 
                var messageData = _currentViewModel.ChatMessages[i];
                var messageView = _messageSlots[i];
                
                // Заново отрисовываем все сообщения, при этом уточняем - нужно ли показывать устаревшие сообщения.
                messageView.Write(ToColoredString(messageData), messageData.IsVisible || showHideMessages);
            }
        }

        private void OnShowHideChanged(bool isShown)
        {
            if (isShown)
            {
                ShowChatInput();
            }
            else
            {
                HideChatInput();
            }
            
            UpdateChatMessages(_isActiveMode);
        }

        private void ShowChatInput()
        {
            _isActiveMode = true;
            
            _inputField.gameObject.SetActive(_isActiveMode);
            _inputField.Select();
            _inputField.ActivateInputField();
        }

        private void HideChatInput()
        {
            _isActiveMode = false;
            
            _currentViewModel.SendMessage(_inputField.text);
            
            _inputField.gameObject.SetActive(_isActiveMode);
            _inputField.text = string.Empty;
        }
        
        public string ToColoredString(ChatMessageData message)
        {
            switch (message.Type)
            {
                case ChatMessageType.None:
                    return $"<color=#333333>{message.Text}</color>";
                case ChatMessageType.Alert:
                    return $"<color=red>{message.Text}</color>";
                case ChatMessageType.System:
                    return $"<color=black>[SYSTEM]</color>: <color=white>{message.Text}</color>";
                case ChatMessageType.User:
                    return $"<color=grey>{message.FromName}</color>: <color=white>{message.Text}</color>";
                case ChatMessageType.Player:
                    return $"<color=blue>{message.FromName}</color>: <color=white>{message.Text}</color>";
                default:
                    break;
            }

            return string.Empty;

        }
    }
}