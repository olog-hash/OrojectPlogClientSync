using System;
using System.Collections.Generic;
using ProjectOlog.Code.UI.HUD.Killbar.Elements;
using R3;

namespace ProjectOlog.Code.UI.HUD.Killbar.Models
{
    public enum KillbarItemType
    {
        Normal,  // Обычное убийство (темно-серый)
        Local,   // Убийство локальным игроком (оранжевый)
        System   // Системное сообщение (другой цвет если нужно)
    }
    
    public class KillbarItemModel : IDisposable
    {
        // Тип записи (определяет цвет фона)
        public KillbarItemType Type { get; set; } = KillbarItemType.Normal;
        
        public List<KillbarElement> Elements { get; } = new List<KillbarElement>();
        public ReactiveProperty<float> LifeTime { get; } = new ReactiveProperty<float>(0f);
        
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