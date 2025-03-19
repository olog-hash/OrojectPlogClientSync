using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Chat.Models
{
    // Часть сообщения
    public class MessagePart
    {
        public string Text { get; set; }
        public Color Color { get; set; } = Color.white;
        public int FontSize { get; set; } = 12;
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
    }

    // Модель сообщения
    public class ChatMessageModel : IDisposable
    {
        public ReactiveProperty<float> LifeTime { get; } = new ReactiveProperty<float>(0f);
        public List<MessagePart> Parts { get; } = new List<MessagePart>();
        
        public bool IsVisible => LifeTime.Value > 0f;
        
        public void UpdateLifetime(float deltaTime)
        {
            if (LifeTime.Value <= 0f) return;
            LifeTime.Value = Math.Max(0f, LifeTime.Value - deltaTime);
        }
        
        public void Dispose()
        {
            LifeTime.Dispose();
        }
    }
}