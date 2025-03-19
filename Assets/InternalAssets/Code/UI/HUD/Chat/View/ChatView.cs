using System.Collections.Generic;
using ObservableCollections;
using ProjectOlog.Code.UI.Core.UIToolkitAddon;
using ProjectOlog.Code.UI.HUD.Chat.Models;
using ProjectOlog.Code.UI.HUD.Chat.Presenter;
using R3;
using UnityEngine;
using UnityEngine.UIElements;

namespace ProjectOlog.Code.UI.HUD.Chat.View
{
    public class ChatView : UIToolkitScreen<ChatViewModel>
    {
        private VisualElement _messageContainer;
        private TextField _textField;
        
        private readonly List<VisualElement> _messageElements = new List<VisualElement>();
        private bool _isInternalTextUpdate;
        private string _lastTextFieldValue = string.Empty;
        
        protected override void SetVisualElements()
        {
            _messageContainer = _root.Q<VisualElement>("message-container");
            _textField = _root.Q<TextField>("text-field");
            
            _textField.style.visibility = Visibility.Hidden;
        }
        
        protected override void OnBind(ChatViewModel model)
        {
            BindInputVisibility(model);
            BindInputText(model);
            BindMessages(model);
        }
        
        protected override void OnUnbind(ChatViewModel model)
        {
            ClearMessages();
        }
        
        private void BindInputVisibility(ChatViewModel model)
        {
            model.IsInputActive
                .Subscribe(isActive =>
                {
                    _textField.style.visibility = isActive ? Visibility.Visible : Visibility.Hidden;
                    
                    if (isActive)
                    {
                        _root.schedule.Execute(() => _textField.Focus()).StartingIn(10);
                    }
                    
                    foreach (var message in _model.Messages)
                    {
                        message.LifeTime.ForceNotify();
                    }
                });
        }
        
        private void BindInputText(ChatViewModel model)
        {
            model.CurrentInputText
                .Subscribe(text =>
                {
                    _isInternalTextUpdate = true;
                    _textField.value = text;
                    _lastTextFieldValue = text;
                    _isInternalTextUpdate = false;
                });
        }
        
        private void BindMessages(ChatViewModel model)
        {
            model.Messages.ObserveAdd()
                .Subscribe(addEvent => AddMessageToUI(addEvent.Value));
            
            model.Messages.ObserveRemove()
                .Subscribe(removeEvent => RemoveMessageFromUI(removeEvent.Index));
            
            foreach (var message in model.Messages)
            {
                AddMessageToUI(message);
            }
        }
        
        private void AddMessageToUI(ChatMessageModel messageModel)
        {
            var messageElement = CreateMessageElement(messageModel);
            _messageContainer.Add(messageElement);
            _messageElements.Add(messageElement);
            
            messageModel.LifeTime
                .Subscribe(lifeTime =>
                {
                    bool isVisible = lifeTime > 0 || _model.IsInputActive.Value;
                    messageElement.style.visibility = isVisible ? Visibility.Visible : Visibility.Hidden;
                });
        }
        
        private VisualElement CreateMessageElement(ChatMessageModel messageModel)
        {
            var messageElement = new VisualElement { name = "Message" };
    
            messageElement.style.flexGrow = 0;
            messageElement.style.flexShrink = 0;
            messageElement.style.flexDirection = FlexDirection.Row;
            messageElement.style.flexWrap = Wrap.Wrap;
            messageElement.style.paddingLeft = 8;
    
            // Создаем элементы для каждой части сообщения
            foreach (var part in messageModel.Parts)
            {
                var label = new Label(part.Text);
                label.style.color = part.Color;
                label.style.fontSize = part.FontSize;
                label.style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(
                    (part.IsBold && part.IsItalic) ? FontStyle.BoldAndItalic :
                    part.IsBold ? FontStyle.Bold :
                    part.IsItalic ? FontStyle.Italic : FontStyle.Normal
                );
                
                // Критичные изменения для устранения лишних отступов
                label.style.marginLeft = 0;
                label.style.marginRight = 0;
                label.style.paddingLeft = 0;
                label.style.paddingRight = 0;
        
                // Сбрасываем white-space, чтобы текст не переносился на новую строку
                label.style.whiteSpace = WhiteSpace.Normal;
        
                // Убираем стандартные отступы Unity
                label.style.marginTop = 0;
                label.style.marginBottom = 0;
                label.style.paddingTop = 2;
                label.style.paddingBottom = 2;
        
                messageElement.Add(label);
            }
    
            return messageElement;
        }
        
        private void RemoveMessageFromUI(int index)
        {
            if (index < 0 || index >= _messageElements.Count) return;
            
            var element = _messageElements[index];
            _messageContainer.Remove(element);
            _messageElements.RemoveAt(index);
        }
        
        private void Update()
        {
            // Отслеживаем изменения текстового поля напрямую вместо колбеков
            if (_model != null && _model.IsInputActive.Value && !_isInternalTextUpdate)
            {
                if (_textField.value != _lastTextFieldValue)
                {
                    _lastTextFieldValue = _textField.value;
                    _model.CurrentInputText.Value = _lastTextFieldValue;
                }
            }
        }
        
        private void ClearMessages()
        {
            foreach (var messageElement in _messageElements)
            {
                _messageContainer.Remove(messageElement);
            }
            
            _messageElements.Clear();
        }
    }
}