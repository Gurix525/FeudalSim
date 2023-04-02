using UnityEngine;

namespace Items
{
    public class ContainerWindow : MonoBehaviour
    {
        [SerializeField] private SortButton _sortButton;

        private Container _container;

        public void Initialize(Container container)
        {
            _container = container;
            _sortButton.Initialize(container);
        }
    }
}