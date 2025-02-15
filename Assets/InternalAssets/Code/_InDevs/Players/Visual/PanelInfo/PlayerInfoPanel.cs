using TMPro;
using UnityEngine;

namespace ProjectOlog.Code._InDevs.Players.Visual.PanelInfo
{
    public class PlayerInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _textMeshPro;

        [SerializeField]
        private Transform _panelTransform;

        public void Initialize(string playername)
        {
            _textMeshPro.text = playername;
        }

        void LateUpdate()
        {
            if (UnityEngine.Camera.main == null || _panelTransform == null) return;

            _panelTransform.LookAt(_panelTransform.position + UnityEngine.Camera.main.transform.forward);
        }

        public void SetActivePanel(bool flag)
        {
            if (_panelTransform == null) return;
            
            _panelTransform.gameObject.SetActive(flag);
        }
    }
}