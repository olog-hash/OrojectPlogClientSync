using System.Collections.Generic;
using ProjectOlog.Code.UI.HUD.KillPanel.Builder.Elements;
using ProjectOlog.Code.UI.HUD.KillPanel.View.Elements;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.KillPanel.View
{
    public class KillBarSlot : MonoBehaviour
    {
        [SerializeField] private Transform _messagesRoot;
        
        [Header("Elements")]
        [SerializeField] private KillBarSlotText _textElement;

        [SerializeField]
        private List<KillBarSlotElement> _elemetsList = new List<KillBarSlotElement>();

        public void Display(KillMessageData data)
        {
            Clear();
            
            for (int i = 0; i < data.Elements.Count; i++)
            {
                ExecuteElement(data.Elements[i]);
            }
        }

        private void ExecuteElement(KillMessageElement elementData)
        {
            if (elementData is KillMessageTextElement textData)
            {
                var textElement = Instantiate(_textElement, _messagesRoot);
                
                textElement.SetText(textData.Text);
                textElement.SetColor(textData.Color);
                textElement.gameObject.SetActive(true);
                
                _elemetsList.Add(textElement);
            }
        }
        
        public void Clear()
        {
            for (int i = 0; i < _elemetsList.Count; i++)
            {
                Destroy(_elemetsList[i].gameObject);
            }
            
            _elemetsList.Clear();
        }
    }
}