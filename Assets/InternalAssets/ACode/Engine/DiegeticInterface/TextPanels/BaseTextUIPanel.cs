using ProjectOlog.Code.Game.DiegeticInterface.LookPanel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectOlog.ACode.Engine.DiegeticInterface.TextPanels
{
    public class BaseTextUIPanel : LookAtCamera
    {
        [SerializeField] protected RectTransform _panelBody;
        [SerializeField] protected TextMeshProUGUI _textMeshPro;
        [SerializeField] protected Image _backgroundImage;
        
        public GameObject GameObject
        {
            get
            {
                if (_gameObject == null)
                {
                    _gameObject = gameObject;
                }
                return _gameObject;
            }
        }
        
        private GameObject _gameObject;
        
        public void WriteText(string text)
        {
            _textMeshPro.text = text;
        }
        
        public void Show()
        {
            GameObject.SetActive(true);
        }

        public void Hide()
        {
            GameObject.SetActive(false);
        }
    }
}