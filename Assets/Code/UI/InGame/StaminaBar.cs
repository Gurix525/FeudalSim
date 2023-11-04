using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] private Image _fill;
        [SerializeField] private TextMeshProUGUI _counter;

        private void Start()
        {
            PlayerControls.Player.Current.Stats.StatsChanged.AddListener(SetFill);
        }

        private void SetFill(Stats stats)
        {
            _fill.fillAmount = stats.CurrentStamina / stats.MaxStamina;
            _counter.text = $"{(int)stats.CurrentStamina}/{(int)stats.MaxStamina}";
        }
    }
}