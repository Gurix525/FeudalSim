using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public abstract class Option : MonoBehaviour
    {
        protected Settings _settings;

        private Toggle _toggle;
        private Slider _slider;
        private TMP_Dropdown _dropdown;

        public string Name => gameObject.name;

        public string Value
        {
            get
            {
                if (_toggle != null)
                    return _toggle.isOn.ToString();
                if (_slider != null)
                    return _slider.value.ToString();
                if (_dropdown != null)
                    return _dropdown.value.ToString();
                return "";
            }
            set
            {
                if (_toggle != null)
                    _toggle.isOn = value == "True" ? true : false;
                if (_slider != null)
                    _slider.value = float.Parse(value);
                if (_dropdown != null)
                    _dropdown.value = int.Parse(value);
            }
        }

        public void Initialize(Settings settings)
        {
            _settings = settings;
            _toggle = GetComponentInChildren<Toggle>(false);
            _slider = GetComponentInChildren<Slider>(false);
            _dropdown = GetComponentInChildren<TMP_Dropdown>(false);

            int activeElements = 0;
            if (_toggle != null)
                activeElements++;
            if (_slider != null)
                activeElements++;
            if (_dropdown != null)
                activeElements++;

            if (activeElements != 1)
                Debug.LogError("Za dużo lub za mało włączonych elementów opcji. " +
                    "Opcja musi mieć zawsze dokładnie jedną z " +
                    "podanych aktywną: toggle / slider / dropdown.");

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