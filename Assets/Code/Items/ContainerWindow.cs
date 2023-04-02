using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace Items
{
    public class ContainerWindow : MonoBehaviour
    {
        [SerializeField] private SortButton _sortButton;

        private Container _container;
        private ContainerHandler _containerHandler;
        private RectTransform _rectTransform;
        public Vector2 CurrentOffset { get; set; }

        private static Vector2 _originalOffset = new(0F, 160F);

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