using Items;
using UnityEngine;

namespace UI
{
    public class ContainerWindow : Window
    {
        [SerializeField] private SortButton _sortButton;

        private Container _container;
        private ContainerHandler _containerHandler;

        public void Initialize(Container container, ContainerHandler containerHandler)
        {
            _container = container;
            _containerHandler = containerHandler;
            _sortButton.Initialize(container);
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            if (_containerHandler != null)
                transform.position = Camera.main.WorldToScreenPoint(_containerHandler.transform.position);
            transform.position += (Vector3)_offset;
        }
    }
}