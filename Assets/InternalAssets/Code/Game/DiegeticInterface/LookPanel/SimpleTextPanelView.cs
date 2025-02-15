using TMPro;
using UnityEngine;

namespace ProjectOlog.Code.Game.DiegeticInterface.LookPanel
{
    public class SimpleTextPanelView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _textMeshPro;

        public void WriteText(string text)
        {
            gameObject.SetActive(true);
            
            _textMeshPro.text = text;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}