using System;
using Buildings;
using Controls;
using Items;
using PlayerControls;
using UnityEngine;

namespace UI
{
    public class CraftingWindow : Window
    {
        [SerializeField] private Transform _itemsList;

        private GameObject _craftingButtonPrefab;

        #region Unity

        private void Awake()
        {
            _craftingButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/CraftingButton");
        }

        private void OnEnable()
        {
            LoadItems();
        }

        #endregion Unity

        #region Private

        private void LoadItems()
        {
            ClearButtons();
            foreach (var item in Item.ItemModels)
            {
                GameObject button = Instantiate(_craftingButtonPrefab, _itemsList);
                button.GetComponent<CraftingButton>().Initialize(item);
            }
        }

        private void LoadFurniture()
        {
        }

        private void ClearButtons()
        {
            for (int i = 0; i < _itemsList.childCount; i++)
            {
                Destroy(_itemsList.GetChild(i).gameObject);
            }
        }

        #endregion Private
    }
}