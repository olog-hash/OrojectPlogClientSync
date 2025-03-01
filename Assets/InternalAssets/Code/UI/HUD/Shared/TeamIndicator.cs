using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectOlog.Code.UI.HUD.Shared
{
    public class TeamIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _firstEnumenator;
        [SerializeField] private TextMeshProUGUI _secondEnumenator;
        [SerializeField] private Image _indicatorBackground;
        [SerializeField] private Sprite[] _teamBackgrounds;

        private float _maxIndicatorValue;
        
        public void SetTeam(int team)
        {
            if (team > _teamBackgrounds.Length) return;

            _indicatorBackground.sprite = _teamBackgrounds[team];
        }

        public void Initialize(float max)
        {
            _maxIndicatorValue = max;
        }

        public void UpdateIndicator(float max, int current, int reserve = 0)
        {
            _indicatorBackground.fillAmount = current / max;
            _firstEnumenator.text = current.ToString();

            if (_secondEnumenator) _secondEnumenator.text = reserve.ToString();
        }

    }
}