using System.Linq;
using Controls;
using Extensions;
using UnityEngine;

namespace UI
{
    public class TooltipWindow : Window
    {
        [SerializeField] private MainCursor _mainCursor;
        [SerializeField] private CanvasGroup _canvasGroup;
        private GameObject _tooltipElementViewPrefab;
        private Tooltip _tooltip;
        private bool _isVisible;

        public void ShowTooltip(Tooltip tooltip)
        {
            if (tooltip == null)
                return;
            if (_tooltip == tooltip || tooltip.Elements.Count() == 0)
                return;
            ClearElements();
            _tooltip = tooltip;
            _isVisible = true;
            foreach (TooltipElement element in tooltip.Elements)
            {
                GameObject view = Instantiate(_tooltipElementViewPrefab, transform);
                view.GetComponent<TooltipElementView>().Initialize(element);
            }
        }

        public void HideTooltip()
        {
            _isVisible = false;
            _tooltip = null;
        }

        private void Awake()
        {
            _tooltipElementViewPrefab = Resources.Load<GameObject>("Prefabs/UI/TooltipElementView");
        }

        private void Update()
        {
            transform.position = _mainCursor.ScreenPosition + new Vector2(16F, -16F);
        }

        private void FixedUpdate()
        {
            _canvasGroup.alpha += _isVisible ? 0.2F : -0.2F;
            _canvasGroup.alpha = _canvasGroup.alpha.Clamp(0F, 1F);
        }

        private void ClearElements()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}