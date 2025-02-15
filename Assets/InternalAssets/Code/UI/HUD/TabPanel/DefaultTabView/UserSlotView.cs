using TMPro;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD.TabPanel.DefaultTabView
{
    public class UserSlotView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _slotIDText;
        [SerializeField] private TextMeshProUGUI _usernameText;
        [SerializeField] private TextMeshProUGUI _killsText;
        [SerializeField] private TextMeshProUGUI _deathsText;
        [SerializeField] private TextMeshProUGUI _pingText;

        private int _slotID;

        public void Initialize(int slotID, UserSlotData userData)
        {
            _slotID = slotID;
            _slotIDText.text = _slotID.ToString();
            
            UpdateSlot(userData);
        }
        
        public void UpdateSlot(UserSlotData userData)
        {
            _usernameText.text = userData.Username;
            _killsText.text = userData.KillCount.ToString();
            _deathsText.text = userData.DeathCount.ToString();
            _pingText.text = userData.Ping.ToString();
        }

        public void Clear()
        {
            _slotIDText.text = string.Empty;
            _usernameText.text = string.Empty;
            _killsText.text = string.Empty;
            _deathsText.text = string.Empty;
            _pingText.text = string.Empty;
        }
    }
}