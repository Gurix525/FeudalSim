using Items;
using UnityEngine;

namespace UI
{
    public class InventoryWindow : Window
    {
        [SerializeField] private SortButton _sortButton;

        private Container _container;
        private InventoryCanvas _equipment;

        public void Initialize(Container container, InventoryCanvas equipment)
        {
            _container = container;
            _equipment = equipment;
            _sortButton.Initialize(container);
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}