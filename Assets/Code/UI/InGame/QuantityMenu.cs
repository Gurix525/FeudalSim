using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuantityMenu : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Slider _slider;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;
    }
}