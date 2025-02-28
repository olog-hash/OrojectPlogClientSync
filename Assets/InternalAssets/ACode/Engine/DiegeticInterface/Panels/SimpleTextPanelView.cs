using TMPro;
using UnityEngine;

namespace ProjectOlog.Code.Game.DiegeticInterface.LookPanel
{
    /// <summary>
    /// Отвечает за отображение текстовой панели в диегетическом интерфейсе
    /// </summary>
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