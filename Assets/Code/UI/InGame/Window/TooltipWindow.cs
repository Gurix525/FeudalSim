using UnityEngine;

namespace UI
{
    public class TooltipWindow : Window
    {
        private GameObject _tooltipElement;

        public Tooltip Tooltip { get; private set; }

        public void ShowTooltip(Tooltip tooltip)
        {
            gameObject.SetActive(true);
            Tooltip = tooltip;
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            _tooltipElement = Resources.Load<GameObject>("Prefabs/UI/TooltipElement");
        }
    }
}