using Items;
using UnityEngine;

namespace UI
{
    public class ArmorWindow : Window
    {
        private Container _container;
        private InventoryCanvas _equipment;

        public void Initialize(Container container, InventoryCanvas equipment)
        {
            _container = container;
            _equipment = equipment;
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}