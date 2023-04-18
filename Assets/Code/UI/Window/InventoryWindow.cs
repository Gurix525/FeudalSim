using Items;
using UnityEngine;

namespace UI
{
    public class InventoryWindow : Window
    {
        [SerializeField] private SortButton _sortButton;

        private Container _container;
        private Equipment _equipment;

        public void Initialize(Container container, Equipment equipment)
        {
            _container = container;
            _equipment = equipment;
            _sortButton.Initialize(container);
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnDisable()
        {
            CurrentOffset = _originalOffset;
        }
    }
}