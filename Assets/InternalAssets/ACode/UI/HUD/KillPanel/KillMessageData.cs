using System.Collections.Generic;
using ProjectOlog.Code.UI.HUD.KillPanel.Builder.Elements;

namespace ProjectOlog.Code.UI.HUD.KillPanel
{
    public class KillMessageData
    {
        public bool IsVisible { get; private set; } = true;
        public float LifeTime { get; private set; }
        public List<KillMessageElement> Elements;

        public KillMessageData(List<KillMessageElement> elements, float lifeTime)
        {
            Elements = elements;
            LifeTime = lifeTime;
        }

        public void OnUpdate(float deltaTime)
        {
            if (!IsVisible) return;

            LifeTime -= deltaTime;
            if (LifeTime <= 0)
            {
                LifeTime = 0;
                IsVisible = false;
            }
        }
    }
}