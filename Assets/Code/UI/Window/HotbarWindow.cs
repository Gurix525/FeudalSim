using System.Collections;
using Input;
using Items;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Cursor = Controls.Cursor;

namespace UI
{
    public class HotbarWindow : Window
    {
        #region Fields

        private HotbarSlot[] _slots;
        private int _selectedSlotIndex;

        #endregion Fields

        #region Public

        public void SetSlotIndex(int slotIndex)
        {
            _selectedSlotIndex = slotIndex;
            SendHotbarItemToCursor();
        }

        #endregion Public

        #region Unity

        private void Start()
        {
            _slots = new HotbarSlot[8];
            for (int i = 0; i < 8; i++)
            {
                _slots[i] = Instantiate(
                    Resources.Load<GameObject>("Prefabs/UI/HotbarSlot"),
                    transform)
                    .GetComponent<HotbarSlot>();
                _slots[i].Initialize(i, Equipment.InventoryContainer, this);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(DelayOnEnable());
        }

        private void OnDisable()
        {
            Equipment.InventoryContainer.CollectionUpdated.RemoveListener(SendHotbarItemToCursor);
            PlayerController.MainHotaber.RemoveListener(ActionType.Started, SetSlotIndexFromKeyboard);
        }

        #endregion Unity

        #region Private

        private void SendHotbarItemToCursor()
        {
            Cursor.HotbarItem = _slots[_selectedSlotIndex].Item;
            Cursor.Container.CollectionUpdated.Invoke();
        }

        private void SetSlotIndexFromKeyboard(CallbackContext context)
        {
            _selectedSlotIndex = (int)context.ReadValue<float>() - 1;
            SendHotbarItemToCursor();
        }

        private IEnumerator DelayOnEnable()
        {
            yield return null;
            Equipment.InventoryContainer.CollectionUpdated.AddListener(SendHotbarItemToCursor);
            PlayerController.MainHotaber.AddListener(ActionType.Started, SetSlotIndexFromKeyboard);
        }

        #endregion Private
    }
}