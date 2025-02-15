using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.KillPanel.Builder.Elements
{
    
    public class KillMessageTextElement : KillMessageElement
    {
        public readonly string Text;
        public readonly Color Color;

        public KillMessageTextElement(string text, Color color)
        {
            Text = text;
            Color = color;
        }
    }
}