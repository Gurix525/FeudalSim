using TMPro;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Items;
using System;

namespace UI
{
    public class HotbarSlot : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        [SerializeField] private Image _background;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _slotNumberText;

        private HotbarWindow _window;
        private int _slotIndex;
        private Container _container;

        #endregion Fields

        #region Properties

        public Item Item => _slotIndex < _container.Size
            ? _container[_slotIndex] : null;

        #endregion Properties

        #region Public

        public void Initialize(int slotIndex, Container container, HotbarWindow window)
        {
            _window = window;
            _slotIndex = slotIndex;
            _slotNumberText.text = (_slotIndex + 1).ToString();
            _container = container;
            _container.CollectionUpdated.AddListener(OnCollectionUpdated);
            _window.SelectedSlotIndexUpdated.AddListener(OnSelectedSlotIndexUpdated);
            _window.SetSlotIndex(0);
            OnCollectionUpdated();
        }

        public void Clear()
        {
            _container.CollectionUpdated.RemoveListener(OnCollectionUpdated);
        }

        // To be added
        //public void OnPointerEnter(PointerEventData eventData)
        //{
        //    PlayerController.MainLeftClick.AddListener(ActionType.Started, OnLeftMouseButton);
        //    PlayerController.MainRightClick.AddListener(ActionType.Started, OnRightMouseButton);
        //}

        //public void OnPointerExit(PointerEventData eventData)
        //{
        //    PlayerController.MainLeftClick.RemoveListener(ActionType.Started, OnLeftMouseButton);
        //    PlayerController.MainRightClick.RemoveListener(ActionType.Started, OnRightMouseButton);
        //}

        #endregion Public

        #region Unity

        // To be added
        //private void OnEnable()
        //{
        //    if (_container != null)
        //        _container.CollectionUpdated.AddListener(OnCollectionUpdated);
        //    if (_window != null)
        //        _window.SelectedSlotIndexUpdated.AddListener(OnSelectedSlotIndexUpdated);
        //}

        //private void OnDisable()
        //{
        //    _container.CollectionUpdated.RemoveListener(OnCollectionUpdated);
        //    _window.SelectedSlotIndexUpdated.RemoveListener(OnSelectedSlotIndexUpdated);
        //    OnPointerExit(null);
        //}

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
            //if (_container[_slotIndex].Count == 1)
            //    _text.text = string.Empty;
            //else
            _text.text = _container[_slotIndex].Count.ToString();
            _image.sprite = _container[_slotIndex].Sprite;
            _image.enabled = true;
        }

        private void OnSelectedSlotIndexUpdated(int slotIndex)
        {
            if (_slotIndex == slotIndex)
                MarkSlotActive();
            else
                MarkSlotInactive();
        }

        private void MarkSlotActive()
        {
            _background.color = new Color(0.8773585F, 0.5344518F, 0.3517711F);
        }

        private void MarkSlotInactive()
        {
            _background.color = new Color(0.9529412F, 0.8705882F, 0.7882353F);
        }

        #endregion Private
    }
}