using System;
using System.Collections;
 
using Items;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.InputSystem.InputAction;
using Cursor = Controls.PlayerCursor;

namespace UI
{
    public class HotbarWindow : Window
    {
        #region Fields

        private HotbarSlot[] _slots;
        public int SelectedSlotIndex { get; private set; }

        #endregion Fields

        #region Properties

        public UnityEvent<int> SelectedSlotIndexUpdated { get; } = new();

        #endregion Properties

        #region Public

        public void SetSlotIndex(int slotIndex)
        {
            SelectedSlotIndex = slotIndex;
            SendHotbarItemToCursor();
            SelectedSlotIndexUpdated.Invoke(slotIndex);
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

        //private void OnEnable()
        //{
        //    StartCoroutine(DelayOnEnable());
        //}

        // To be added
        //private void OnDisable()
        //{
        //    Equipment.InventoryContainer.CollectionUpdated.RemoveListener(SendHotbarItemToCursor);
        //    PlayerController.MainHotbar1.RemoveListener(ActionType.Started, OnHotbar1);
        //    PlayerController.MainHotbar2.RemoveListener(ActionType.Started, OnHotbar2);
        //    PlayerController.MainHotbar3.RemoveListener(ActionType.Started, OnHotbar3);
        //    PlayerController.MainHotbar4.RemoveListener(ActionType.Started, OnHotbar4);
        //    PlayerController.MainHotbar5.RemoveListener(ActionType.Started, OnHotbar5);
        //    PlayerController.MainHotbar6.RemoveListener(ActionType.Started, OnHotbar6);
        //    PlayerController.MainHotbar7.RemoveListener(ActionType.Started, OnHotbar7);
        //    PlayerController.MainHotbar8.RemoveListener(ActionType.Started, OnHotbar8);
        //}

        #endregion Unity

        #region Private

        private void SendHotbarItemToCursor()
        {
            throw new NotImplementedException();
            //Cursor.HotbarItem = _slots[SelectedSlotIndex].Item;
            //Cursor.Container.CollectionUpdated.Invoke();
        }

        // To be added
        //private IEnumerator DelayOnEnable()
        //{
        //    yield return null;
        //    Equipment.InventoryContainer.CollectionUpdated.AddListener(SendHotbarItemToCursor);
        //    PlayerController.MainHotbar1.AddListener(ActionType.Started, OnHotbar1);
        //    PlayerController.MainHotbar2.AddListener(ActionType.Started, OnHotbar2);
        //    PlayerController.MainHotbar3.AddListener(ActionType.Started, OnHotbar3);
        //    PlayerController.MainHotbar4.AddListener(ActionType.Started, OnHotbar4);
        //    PlayerController.MainHotbar5.AddListener(ActionType.Started, OnHotbar5);
        //    PlayerController.MainHotbar6.AddListener(ActionType.Started, OnHotbar6);
        //    PlayerController.MainHotbar7.AddListener(ActionType.Started, OnHotbar7);
        //    PlayerController.MainHotbar8.AddListener(ActionType.Started, OnHotbar8);
        //}

        private void OnHotbar1(CallbackContext context)
        {
            SelectedSlotIndex = 0;
            SendHotbarItemToCursor();
            SelectedSlotIndexUpdated.Invoke(0);
        }

        private void OnHotbar2(CallbackContext context)
        {
            SelectedSlotIndex = 1;
            SendHotbarItemToCursor();
            SelectedSlotIndexUpdated.Invoke(1);
        }

        private void OnHotbar3(CallbackContext context)
        {
            SelectedSlotIndex = 2;
            SendHotbarItemToCursor();
            SelectedSlotIndexUpdated.Invoke(2);
        }

        private void OnHotbar4(CallbackContext context)
        {
            SelectedSlotIndex = 3;
            SendHotbarItemToCursor();
            SelectedSlotIndexUpdated.Invoke(3);
        }

        private void OnHotbar5(CallbackContext context)
        {
            SelectedSlotIndex = 4;
            SendHotbarItemToCursor();
            SelectedSlotIndexUpdated.Invoke(4);
        }

        private void OnHotbar6(CallbackContext context)
        {
            SelectedSlotIndex = 5;
            SendHotbarItemToCursor();
            SelectedSlotIndexUpdated.Invoke(5);
        }

        private void OnHotbar7(CallbackContext context)
        {
            SelectedSlotIndex = 6;
            SendHotbarItemToCursor();
            SelectedSlotIndexUpdated.Invoke(6);
        }

        private void OnHotbar8(CallbackContext context)
        {
            SelectedSlotIndex = 7;
            SendHotbarItemToCursor();
            SelectedSlotIndexUpdated.Invoke(7);
        }

        #endregion Private
    }
}