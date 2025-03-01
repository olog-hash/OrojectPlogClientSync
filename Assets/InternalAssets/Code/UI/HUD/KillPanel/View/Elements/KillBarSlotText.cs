using TMPro;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.KillPanel.View.Elements
{
    public class KillBarSlotText : KillBarSlotElement
    {
        [SerializeField] private TextMeshProUGUI _message;

        public void SetText(string text)
        {
            _message.text = text;
        }
        
        public void SetColor(Color color)
        {
            _message.color = color;
        }

        public void Clear()
        {
            _message.text = string.Empty;
            gameObject.SetActive(false);
        }
    }
}