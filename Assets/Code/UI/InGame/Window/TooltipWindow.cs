using System.Collections;
using System.Linq;
using Controls;
using Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TooltipWindow : Window
    {
        [SerializeField] private MainCursor _mainCursor;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private VerticalLayoutGroup _verticalLayoutGroup;
        private GameObject _tooltipElementViewPrefab;
        private Tooltip _tooltip;
        private bool _isVisible;
        private bool _isAlphaLocked = false;

        public void ShowTooltip(Tooltip tooltip)
        {
            if (tooltip == null)
                return;
            if (_tooltip == tooltip || tooltip.Elements.Count() == 0)
                return;
            ClearElements();
            _canvasGroup.alpha = 0F;
            _tooltip = tooltip;
            _isVisible = true;
            foreach (TooltipElement element in tooltip.Elements)
            {
                GameObject view = Instantiate(_tooltipElementViewPrefab, transform);
                view.GetComponent<TooltipElementView>().Initialize(element);
            }
            StartCoroutine(RefreshVerticalLayoutGroup());
        }

        public void HideTooltip()
        {
            _isVisible = false;
            _tooltip = null;
        }

        private void Awake()
        {
            _tooltipElementViewPrefab = Resources.Load<GameObject>("Prefabs/UI/TooltipElementView");
            _verticalLayoutGroup = GetComponent<VerticalLayoutGroup>();
        }

        private void Update()
        {
            transform.position = _mainCursor.ScreenPosition + new Vector2(16F, -16F);
        }

        private void FixedUpdate()
        {
            if (!_isAlphaLocked)
            {
            _canvasGroup.alpha += _isVisible ? 0.2F : -0.2F;
            _canvasGroup.alpha = _canvasGroup.alpha.Clamp(0F, 1F);

            }
        }

        private void ClearElements()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        private IEnumerator RefreshVerticalLayoutGroup()
        {
            _isAlphaLocked = true;
            float oldAlpha = _canvasGroup.alpha;
            _canvasGroup.alpha = 0F;
            _verticalLayoutGroup.enabled = false;
            yield return null;
            _verticalLayoutGroup.enabled = true;
            _canvasGroup.alpha = oldAlpha;
            _isAlphaLocked = false;
        }
    }
}