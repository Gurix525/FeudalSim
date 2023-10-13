using System;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class QuantityMenu : MonoBehaviour
    {
        public event EventHandler<FloatChangedEventArgs> QuantityChanged;

        public event EventHandler<FloatChangedEventArgs> MaxQuantityChanged;

        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Slider _slider;
        [SerializeField] private Button _confirmButton;
        [SerializeField] private Button _cancelButton;

        private float _quantity;
        private int _maxQuantity = 2;

        private Container _sourceContainer;
        private Container _destinationContainer;
        private int _sourceIndex;
        private int _destinationIndex;

        public float Quantity
        {
            get => _quantity;
            private set
            {
                if (_quantity != value)
                {
                    FloatChangedEventArgs eventArgs = new(_quantity, value);
                    _quantity = value;
                    QuantityChanged?.Invoke(this, eventArgs);
                }
            }
        }

        public int MaxQuantity
        {
            get => _maxQuantity;
            set
            {
                if (_maxQuantity != value)
                {
                    FloatChangedEventArgs eventArgs = new(_maxQuantity, value);
                    _quantity = value;
                    MaxQuantityChanged?.Invoke(this, eventArgs);
                }
            }
        }

        public static QuantityMenu Current { get; private set; }

        public void Show(Container source, int sourceIndex, Container destination, int destinationIndex, Vector2 screenPosition)
        {
            transform.position = screenPosition;
            gameObject.SetActive(true);
            _maxQuantity = source[sourceIndex].Count;
            _slider.maxValue = _maxQuantity;
            _slider.value = _maxQuantity / 2;
            _sourceContainer = source;
            _destinationContainer = destination;
            _sourceIndex = sourceIndex;
            _destinationIndex = destinationIndex;
        }

        private void Awake()
        {
            Current = this;
            _inputField.characterValidation = TMP_InputField.CharacterValidation.Digit;
            _inputField.characterLimit = 4;
            _slider.maxValue = _maxQuantity;
            _inputField.onValueChanged.AddListener(_inputField_onValueChanged);
            _slider.onValueChanged.AddListener(_slider_onValueChanged);
            _confirmButton.Clicked += _confirmButton_Clicked;
            _cancelButton.Clicked += _cancelButton_Clicked;
            QuantityChanged += QuantityMenu_QuantityChanged;
            MaxQuantityChanged += QuantityMenu_MaxQuantityChanged;
            gameObject.SetActive(false);
        }

        private void _inputField_onValueChanged(string value)
        {
            if (int.TryParse(value, out int result))
            {
                if (result > _maxQuantity)
                {
                    result = _maxQuantity;
                    _inputField.text = result.ToString();
                }
                Quantity = result;
            }
            else
            {
                _inputField.text = "1";
                Quantity = 1F;
            }
        }

        private void _slider_onValueChanged(float value)
        {
            Quantity = value;
        }

        private void _confirmButton_Clicked(object sender, EventArgs e)
        {
            if (_destinationContainer[_destinationIndex] == null)
                Container.PushItemDestructive(_sourceContainer, _sourceIndex, _destinationContainer, _destinationIndex, (int)Quantity);
            else
                Container.MergeItems(_sourceContainer, _sourceIndex, _destinationContainer, _destinationIndex, (int)Quantity);
            gameObject.SetActive(false);
        }

        private void _cancelButton_Clicked(object sender, EventArgs e)
        {
            gameObject.SetActive(false);
        }

        private void QuantityMenu_QuantityChanged(object sender, FloatChangedEventArgs e)
        {
            if (_inputField.text != e.NewValue.ToString())
                _inputField.text = e.NewValue.ToString();
            if (_slider.value != e.NewValue)
                _slider.value = e.NewValue;
        }

        private void QuantityMenu_MaxQuantityChanged(object sender, FloatChangedEventArgs e)
        {
            _slider.maxValue = e.NewValue;
        }
    }
}