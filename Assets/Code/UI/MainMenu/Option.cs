using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class Option : MonoBehaviour
    {
        protected Settings _settings;

        private Toggle _toggle;
        private Slider _slider;
        private Dropdown _dropdown;

        public void Initialize(Settings settings)
        {
            _settings = settings;
            _toggle = GetComponentInChildren<Toggle>();
            _slider = GetComponentInChildren<Slider>();
            _dropdown = GetComponentInChildren<Dropdown>();

            if (_toggle != null)
                _toggle.onValueChanged.AddListener(ExecuteToggle);
            if (_slider != null)
                _slider.onValueChanged.AddListener(ExecuteSlider);
            if (_dropdown != null)
                _dropdown.onValueChanged.AddListener(ExecuteDropdown);
        }

        public void Execute()
        {
            if (_toggle != null)
                ExecuteToggle(_toggle.isOn);
            if (_slider != null)
                ExecuteSlider(_slider.value);
            if (_dropdown != null)
                ExecuteDropdown(_dropdown.value);
        }

        protected virtual void ExecuteToggle(bool arg)
        { }

        protected virtual void ExecuteSlider(float arg)
        { }

        protected virtual void ExecuteDropdown(int arg)
        { }
    }
}