using Input;
using Misc;
using UI;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.EventSystems;
using System;

namespace Items
{
    public class Equipment : MonoBehaviour
    {
        #region Fields

        private Container _armorContainer = new(4, isArmor: true);
        private Container _inventoryContainer = new(4);
        private GameObject[] _armorSlots;
        private GameObject _armorWindow;
        private GameObject[] _inventorySlots = new GameObject[0];
        private GameObject _inventoryWindow;

        #endregion Fields

        #region Properties

        private int InventorySlotCount
        {
            get
            {
                int count = 0;
                foreach (var item in _armorContainer)
                {
                    if (item == null)
                        continue;
                    item.Stats.TryGetValue("InventorySlots", out string value);
                    if (value != null)
                        count += int.Parse(value);
                }
                return 4 + count;
            }
        }

        private static Equipment Instance { get; set; }

        #endregion Properties

        #region Public

        public static void Insert(Item item)
        {
            Instance._inventoryContainer.Insert(item);
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _armorWindow = References.GetReference("ArmorWindow");
            _armorWindow.GetComponent<ArmorWindow>().Initialize(_armorContainer, this);
            _armorWindow.SetActive(false);
            _armorSlots = new GameObject[4];
            for (int i = 0; i < 4; i++)
            {
                _armorSlots[i] = Instantiate(
                    Prefabs.GetPrefab("ContainerSlot"),
                    _armorWindow.transform);
                var containerSlot = _armorSlots[i].GetComponent<ContainerSlot>();
                containerSlot.Initialize(i, _armorContainer);
            }

            _inventoryWindow = References.GetReference("InventoryWindow");
            _inventoryWindow.GetComponent<InventoryWindow>().Initialize(_inventoryContainer, this);
            _inventoryWindow.SetActive(false);
            _armorContainer.CollectionUpdated.AddListener(OnArmorCollectionUpdated);
            CreateInventorySlots();
        }

        private void OnEnable()
        {
            PlayerController.MainTab.AddListener(ActionType.Started, SwitchEquipmentState);
        }

        private void OnDisable()
        {
            PlayerController.MainTab.RemoveListener(ActionType.Started, SwitchEquipmentState);
        }

        #endregion Unity

        #region Private

        private void OnArmorCollectionUpdated()
        {
            _inventoryContainer.ChangeSize(InventorySlotCount, transform.position);
            for (int i = 0; i < _inventorySlots.Length; i++)
            {
                _inventorySlots[i].GetComponent<ContainerSlot>().Clear();
                Destroy(_inventorySlots[i]);
            }
            CreateInventorySlots();
        }

        private void CreateInventorySlots()
        {
            _inventorySlots = new GameObject[InventorySlotCount];
            for (int i = 0; i < InventorySlotCount; i++)
            {
                _inventorySlots[i] = Instantiate(
                    Prefabs.GetPrefab("ContainerSlot"),
                    _inventoryWindow.transform);
                var containerSlot = _inventorySlots[i].GetComponent<ContainerSlot>();
                containerSlot.Initialize(i, _inventoryContainer);
            }
        }

        private void SwitchEquipmentState(CallbackContext context)
        {
            if (_armorWindow.activeInHierarchy)
                HideEquipment(new());
            else
                ShowEquipment();
        }

        private void ShowEquipment()
        {
            _inventoryWindow.SetActive(true);
            _armorWindow.SetActive(true);
            PlayerController.MainEscape.AddListener(ActionType.Started, HideEquipment);
            PlayerController.MainTab.AddListener(ActionType.Started, HideEquipment);
        }

        private void HideEquipment(CallbackContext context)
        {
            PlayerController.MainEscape.RemoveListener(ActionType.Started, HideEquipment);
            PlayerController.MainTab.RemoveListener(ActionType.Started, HideEquipment);
            _armorWindow.SetActive(false);
            _inventoryWindow.SetActive(false);
        }

        #endregion Private
    }
}