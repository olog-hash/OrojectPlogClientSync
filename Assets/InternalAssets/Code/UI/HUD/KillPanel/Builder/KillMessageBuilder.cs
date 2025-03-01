using System.Collections.Generic;
using ProjectOlog.Code.UI.HUD.KillPanel.Builder.Elements;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.KillPanel.Builder
{
    public class KillMessageBuilder
    {
        private List<KillMessageElement> _elements = new List<KillMessageElement>();

        public KillMessageBuilder AddTextElement(string text, Color color)
        {
            _elements.Add(new KillMessageTextElement(text, color));
            return this;
        }

        public KillMessageBuilder AddImageElement(Sprite sprite)
        {
            _elements.Add(new KillMessageImageElement(sprite));
            return this;
        }

        public KillMessageData Build(float lifeTime)
        {
            return new KillMessageData(_elements, lifeTime);
        }
    }

}