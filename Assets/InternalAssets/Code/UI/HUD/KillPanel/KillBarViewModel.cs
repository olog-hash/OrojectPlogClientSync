using System;
using System.Collections.Generic;
using ProjectOlog.Code.UI.Core;

namespace ProjectOlog.Code.UI.HUD.KillPanel
{
    public class KillBarViewModel : BaseViewModel
    {
        public const int MAX_COUNT = 5;

        public Action OnKillMessageReceived;
        public Action OnKillMessageVisibilityChanged;

        public readonly List<KillMessageData> KillMessages;

        public KillBarViewModel()
        {
            KillMessages = new List<KillMessageData>();
        }

        public void AddKillMessage(KillMessageData killMessage)
        {
            KillMessages.Insert(0, killMessage);

            if (KillMessages.Count > MAX_COUNT)
            {
                KillMessages.RemoveAt(MAX_COUNT);
            }

            OnKillMessageReceived?.Invoke();
        }

        public void OnUpdate(float deltaTime)
        {
            bool visibilityChanged = false;
            foreach (var message in KillMessages)
            {
                bool prevVisibility = message.IsVisible;
                message.OnUpdate(deltaTime);
                if (prevVisibility != message.IsVisible)
                    visibilityChanged = true;
            }

            if (visibilityChanged)
            {
                OnKillMessageVisibilityChanged?.Invoke();
            }
        }
    }
}