using TMPro;
using UnityEngine;

namespace ProjectOlog.Code.Engine.DiegeticInterface.Panels
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