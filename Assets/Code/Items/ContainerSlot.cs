using Input;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using Controls;
using static UnityEngine.InputSystem.InputAction;

namespace Items
{
    public class ContainerSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Fields

        private int _slotIndex;
        private Container _container;

        #endregion Fields

        #region Public

        public void Initialize(int slotIndex, Container container)
        {
            _slotIndex = slotIndex;
            _container = container;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerController.MainUse.AddListener(ActionType.Started, OnMainUse);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerController.MainUse.RemoveListener(ActionType.Started, OnMainUse);
        }

        #endregion Public

        #region Private

        private void OnMainUse(CallbackContext context)
        {
            _container.ExchangeItem(_slotIndex, Controls.Cursor.Container, 0);
            Debug.Log(Controls.Cursor.Container[0]);
        }

        #endregion Private
    }
}