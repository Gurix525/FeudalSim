using Input;
using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Items;

namespace UI
{
    public class HotbarSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _slotNumberText;

        private HotbarWindow _window;
        private int _slotIndex;
        private Container _container;

        #endregion Fields

        #region Properties

        public Item Item => _container[_slotIndex];

        #endregion Properties

        #region Public

        public void Initialize(int slotIndex, Container container, HotbarWindow window)
        {
            _window = window;
            _slotIndex = slotIndex;
            _slotNumberText.text = _slotIndex.ToString();
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
            OnPointerExit(null);
        }

        #endregion Unity

        #region Private

        private void OnLeftMouseButton(CallbackContext context)
        {
            _window.SetSlotIndex(_slotIndex);
        }

        private void OnRightMouseButton(CallbackContext context)
        {
            _window.SetSlotIndex(_slotIndex);
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