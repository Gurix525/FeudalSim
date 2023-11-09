using System.Collections.Generic;
using Items;
using UnityEngine;

namespace UI
{
    public class CraftingWindow : Window
    {
        #region Fields

        [SerializeField] private Transform _itemsList;

        private GameObject _craftingButtonPrefab;
        private CraftingButton[] _buttons;

        #endregion Fields

        #region Unity

        private void Awake()
        {
            _craftingButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/CraftingButton");
        }

        private void Start()
        {
            LoadItems();
        }

        private void OnEnable()
        {
            InventoryCanvas.InventoryContainer.CollectionUpdated
                .AddListener(InventoryContainer_CollectionUpdated);
        }

        private void OnDisable()
        {
            InventoryCanvas.InventoryContainer.CollectionUpdated
                .RemoveListener(InventoryContainer_CollectionUpdated);
        }

        #endregion Unity

        #region Private

        private void LoadItems()
        {
            ClearButtons();
            List<CraftingButton> buttons = new();
            foreach (var item in Item.ItemModels)
            {
                if (item.Recipe.IsEmpty)
                    continue;
                GameObject button = Instantiate(_craftingButtonPrefab, _itemsList);
                CraftingButton craftingButton = button.GetComponent<CraftingButton>();
                craftingButton.Initialize(item);
                buttons.Add(craftingButton);
            }
            _buttons = buttons.ToArray();
            UpdateCounters();
        }

        private void ClearButtons()
        {
            for (int i = 0; i < _itemsList.childCount; i++)
            {
                Destroy(_itemsList.GetChild(i).gameObject);
            }
        }

        private void UpdateCounters()
        {
            foreach (var button in _buttons)
                button.UpdateCounter();
        }

        private void InventoryContainer_CollectionUpdated()
        {
            UpdateCounters();
        }

        #endregion Private
    }
}