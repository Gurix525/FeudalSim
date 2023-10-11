using Items;
using UnityEngine;

namespace UI
{
    public class ArmorWindow : Window
    {
        private Container _container;
        private Equipment _equipment;

        public void Initialize(Container container, Equipment equipment)
        {
            _container = container;
            _equipment = equipment;
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}