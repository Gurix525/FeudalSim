using Input;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;
using TMPro;
using UnityEngine.UI;
using System;

namespace Items
{
    public class ContainerSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerController.MainUse.AddListener(ActionType.Started, OnMainUse);
            PlayerController.MainRightClick.AddListener(ActionType.Performed, OnMainRightClick);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerController.MainUse.RemoveListener(ActionType.Started, OnMainUse);
            PlayerController.MainRightClick.RemoveListener(ActionType.Performed, OnMainRightClick);
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

        private void OnMainUse(CallbackContext context)
        {
            _container.HandleLeftClick(_slotIndex);
        }

        private void OnMainRightClick(CallbackContext obj)
        {
            _container.HandleRightClick(_slotIndex);
        }

        private void OnCollectionUpdated()
        {
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