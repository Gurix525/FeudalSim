using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private TextMeshProUGUI _counter;

        private void Start()
        {
            PlayerControls.Player.Instance.Stats.StatsChanged.AddListener(SetFill);
        }

        private void SetFill(Stats stats)
        {
            _fill.fillAmount = stats.CurrentHP / stats.MaxHP;
            _counter.text = $"{(int)stats.CurrentHP}/{(int)stats.MaxHP}";
        }
    }
}