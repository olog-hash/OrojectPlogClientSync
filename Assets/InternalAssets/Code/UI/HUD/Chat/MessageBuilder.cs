using System;
using ProjectOlog.Code.UI.HUD.Chat.Models;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Chat
{
    // Билдер для создания сообщений
    public class MessageBuilder
    {
        private readonly ChatMessageModel _message = new ChatMessageModel();
        
        // Стандартные цвета
        public static readonly Color DefaultColor = Color.white;
        public static readonly Color GrayColor = Color.gray;
        public static readonly Color SystemColor = new Color(1f, 0.8f, 0f);
        public static readonly Color ErrorColor = new Color(1f, 0.3f, 0.3f);
        public static readonly Color SuccessColor = new Color(0.3f, 1f, 0.3f);
        public static readonly Color PlayerNameColor = new Color(0f, 0.4f, 0.8f);
        
        public MessageBuilder()
        {
            _message.LifeTime.Value = 5f; // По умолчанию
        }
        
        // Добавить обычный текст
        public MessageBuilder AddText(string text)
        {
            _message.Parts.Add(new MessagePart { 
                Text = text, 
                Color = DefaultColor 
            });
            return this;
        }
        
        // Добавить окрашенный текст
        public MessageBuilder AddColoredText(string text, Color color)
        {
            _message.Parts.Add(new MessagePart { 
                Text = text, 
                Color = color 
            });
            return this;
        }
        
        // Добавить жирный текст
        public MessageBuilder AddBoldText(string text, Color color = default)
        {
            _message.Parts.Add(new MessagePart { 
                Text = text, 
                Color = color == default ? DefaultColor : color,
                IsBold = true
            });
            return this;
        }
        
        // Добавить курсивный текст
        public MessageBuilder AddItalicText(string text, Color color = default)
        {
            _message.Parts.Add(new MessagePart { 
                Text = text, 
                Color = color == default ? DefaultColor : color,
                IsItalic = true
            });
            return this;
        }
        
        // Добавить системный текст (желтый)
        public MessageBuilder AddSystemText(string text)
        {
            return AddColoredText(text, SystemColor);
        }
        
        // Добавить серый текст
        public MessageBuilder AddGrayText(string text)
        {
            return AddColoredText(text, GrayColor);
        }
        
        // Добавить имя игрока
        public MessageBuilder AddPlayerName(string playerName)
        {
            _message.Parts.Add(new MessagePart { 
                Text = playerName, 
                Color = PlayerNameColor,
                IsBold = true
            });
            return this;
        }
        
        // Установить время жизни сообщения
        public MessageBuilder WithLifetime(float lifetime)
        {
            _message.LifeTime.Value = lifetime;
            return this;
        }
        
        // Создать готовое сообщение
        public ChatMessageModel Build()
        {
            if (_message.Parts.Count == 0)
            {
                throw new InvalidOperationException("Сообщение должно содержать хотя бы одну часть.");
            }
            return _message;
        }
        
        // Вспомогательные методы для типичных сообщений
        
        // Сообщение от игрока: [Имя]: Текст
        public static ChatMessageModel PlayerMessage(string playerName, string text, float lifetime = 5f)
        {
            return new MessageBuilder()
                .AddPlayerName(playerName)
                .AddColoredText(": ", DefaultColor)
                .AddBoldText(text)
                .WithLifetime(lifetime)
                .Build();
        }
        
        // Системное сообщение
        public static ChatMessageModel SystemMessage(string text, float lifetime = 8f)
        {
            return new MessageBuilder()
                .AddBoldText("[СИСТЕМА]", Color.black)
                .AddColoredText(": ", DefaultColor)
                .AddBoldText(text)
                .WithLifetime(lifetime)
                .Build();
        }
        
        // Сообщение об ошибке
        public static ChatMessageModel ErrorMessage(string text, float lifetime = 8f)
        {
            return new MessageBuilder()
                .AddColoredText(text, ErrorColor)
                .WithLifetime(lifetime)
                .Build();
        }
        
        // Сообщение об успехе
        public static ChatMessageModel SuccessMessage(string text, float lifetime = 5f)
        {
            return new MessageBuilder()
                .AddColoredText("[УСПЕХ] ", SuccessColor)
                .AddColoredText(text, SuccessColor)
                .WithLifetime(lifetime)
                .Build();
        }
    }
}