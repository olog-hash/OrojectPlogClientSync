using System;
using ProjectOlog.Code.UI.HUD.Killbar.Elements;
using ProjectOlog.Code.UI.HUD.Killbar.Models;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Killbar
{
    // Гибкий билдер для создания записей в Killbar
    public class KillbarBuilder
    {
        private readonly KillbarItemModel _item = new KillbarItemModel();
        
        public KillbarBuilder()
        {
            _item.LifeTime.Value = 5f; // По умолчанию 5 секунд
        }
        
        // Добавить простой текст
        public KillbarBuilder AddText(string text, Color? color = null)
        {
            var textElement = new TextElement
            {
                Text = text,
                TextColor = color ?? Color.white
            };
            
            _item.Elements.Add(textElement);
            return this;
        }
        
        // Добавить форматированный текст
        public KillbarBuilder AddFormattedText(string text, Color color, int fontSize = 12, 
                                               bool isBold = false, bool isItalic = false, 
                                               float marginLeft = 0f, float marginRight = 0f)
        {
            var textElement = new TextElement
            {
                Text = text,
                TextColor = color,
                FontSize = fontSize,
                IsBold = isBold,
                IsItalic = isItalic,
                MarginLeft = marginLeft,
                MarginRight = marginRight
            };
            
            _item.Elements.Add(textElement);
            return this;
        }
        
        // Добавить жирный текст
        public KillbarBuilder AddBoldText(string text, Color? color = null, 
                                          float marginLeft = 0f, float marginRight = 4f)
        {
            var textElement = new TextElement
            {
                Text = text,
                TextColor = color ?? Color.white,
                IsBold = true,
                MarginLeft = marginLeft,
                MarginRight = marginRight
            };
            
            _item.Elements.Add(textElement);
            return this;
        }
        
        // Добавить изображение
        // Добавить изображение
        public KillbarBuilder AddImage(string imagePath, float width = 61f, float height = 15f,
            float marginLeft = 0f, float marginRight = 4f, bool flexGrow = true)
        {
            var imageElement = new ImageElement
            {
                ImagePath = imagePath,
                Width = width,
                Height = height,
                MarginLeft = marginLeft,
                MarginRight = marginRight,
                FlexGrow = flexGrow
            };
    
            _item.Elements.Add(imageElement);
            return this;
        }
        
        // Установить тип записи (влияет на цвет фона)
        public KillbarBuilder SetType(KillbarItemType type)
        {
            _item.Type = type;
            return this;
        }
        
        // Установить время жизни записи
        public KillbarBuilder WithLifetime(float lifetime)
        {
            _item.LifeTime.Value = lifetime;
            return this;
        }
        
        // Отметить как действие локального игрока (оранжевый фон)
        public KillbarBuilder AsLocalPlayer()
        {
            _item.Type = KillbarItemType.Local;
            return this;
        }
        
        // Создать готовую запись
        public KillbarItemModel Build()
        {
            if (_item.Elements.Count == 0)
            {
                throw new InvalidOperationException("Полоска Killbar должна содержать хотя бы один элемент.");
            }
            
            return _item;
        }
        
        // Вспомогательные методы для быстрого создания типичных записей
        
        // Типичное убийство: Killer → Weapon → Victim
        public static KillbarItemModel StandardKill(string killerName, string weaponIconPath, string victimName, 
                                                   float lifetime = 5f, bool isLocalPlayer = false)
        {
            Color textColor = isLocalPlayer ? Color.black : Color.white;
            
            return new KillbarBuilder()
                .AddBoldText(killerName, textColor)
                .AddImage(weaponIconPath)
                .AddBoldText(victimName, textColor)
                .SetType(isLocalPlayer ? KillbarItemType.Local : KillbarItemType.Normal)
                .WithLifetime(lifetime)
                .Build();
        }
        
        // Самоубийство
        public static KillbarItemModel Suicide(string playerName, string iconPath, 
                                              float lifetime = 5f, bool isLocalPlayer = false)
        {
            Color textColor = isLocalPlayer ? Color.black : Color.white;
            
            return new KillbarBuilder()
                .AddFormattedText(playerName, textColor, marginLeft: 0, marginRight: 0)
                .AddImage(iconPath, marginLeft: 0, marginRight: 0)
                .SetType(isLocalPlayer ? KillbarItemType.Local : KillbarItemType.Normal)
                .WithLifetime(lifetime)
                .Build();
        }
    }
}