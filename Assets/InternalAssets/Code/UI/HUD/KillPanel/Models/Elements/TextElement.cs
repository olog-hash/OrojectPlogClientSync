using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.Killbar.Elements
{
    // Текстовый элемент
    public class TextElement : KillbarElement
    {
        public string Text { get; set; }
        public Color TextColor { get; set; } = Color.white;
        public int FontSize { get; set; } = 12;
        public bool IsBold { get; set; } = false;
        public bool IsItalic { get; set; } = false;
    }
}