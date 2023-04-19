using Input;
using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Items;

namespace UI
{
    public class HotbarSlot : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;

        private int _slotIndex;
        private Container _container;

        #endregion Fields

        #region Public

        public void Initialize(int slotIndex, Container container)
        {
            _slotIndex = slotIndex;
            _container = container;
            _container.CollectionUpdated.AddListener(OnCollectionUpdated);
            OnCollectionUpdated();
        }

        public void Clear()
        {
            _container.CollectionUpdated.RemoveListener(OnCollectionUpdated);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerController.MainUse.AddListener(ActionType.Started, OnLeftMouseButton);
            PlayerController.MainRightClick.AddListener(ActionType.Started, OnRightMouseButton);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerController.MainUse.RemoveListener(ActionType.Started, OnLeftMouseButton);
            PlayerController.MainRightClick.RemoveListener(ActionType.Started, OnRightMouseButton);
        }

        #endregion Public

        #region Unity

        private void OnEnable()
        {
            if (_container != null)
                _container.CollectionUpdated.AddListener(OnCollectionUpdated);
        }

        private void OnDisable()
        {
            _container.CollectionUpdated.RemoveListener(OnCollectionUpdated);
        }

        #endregion Unity

        #region Private

        private void OnLeftMouseButton(CallbackContext context)
        {
            //_container.OnLeftMouseButton(_slotIndex);
        }

        private void OnRightMouseButton(CallbackContext context)
        {
            //_container.OnRightMouseButton(_slotIndex);
        }

        private void OnCollectionUpdated()
        {
            if (_container.Size <= _slotIndex)
            {
                _text.text = string.Empty;
                _image.enabled = false;
                return;
            }
            if (_container[_slotIndex] == null)
            {
                _text.text = string.Empty;
                _image.enabled = false;
                return;
            }
            if (_container[_slotIndex].MaxStack == 1)
                _text.text = string.Empty;
            else
                _text.text = _container[_slotIndex].Count.ToString();
            _image.sprite = _container[_slotIndex].Sprite;
            _image.enabled = true;
        }

        #endregion Private
    }
}