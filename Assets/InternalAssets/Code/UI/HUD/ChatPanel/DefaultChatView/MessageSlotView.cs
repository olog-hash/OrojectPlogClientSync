using TMPro;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.ChatPanel.DefaultChatView
{
    public class MessageSlotView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _message;

        public void Write(string text, bool isDisplayed)
        {
            _message.text = text;
            gameObject.SetActive(isDisplayed);
        }

        public void Clear()
        {
            _message.text = string.Empty;
            gameObject.SetActive(false);
        }
    }
}