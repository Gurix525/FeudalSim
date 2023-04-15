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

        private void Update()
        {
            _rectTransform.position =
                (Vector2)Camera.main.WorldToScreenPoint(_containerHandler.transform.position)
                + CurrentOffset;
        }

        private void OnDisable()
        {
            CurrentOffset = _originalOffset;
        }
    }
}