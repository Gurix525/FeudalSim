using Buildings;
using Controls;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingButton : Button, ITooltipSource
    {
        [SerializeField] private Image _buildingImage;
        [SerializeField] private TextMeshProUGUI _counter;
        [SerializeField] private GameObject _buttonBlock;

        private Recipe _recipe;

        public GameObject BuildingPrefab { get; private set; }

        protected override void Execute()
        {
            base.Execute();
            BuildingCursor.Current.BuildingPrefab = BuildingPrefab;
        }

        public void Initialize(GameObject buildingPrefab)
        {
            BuildingPrefab = buildingPrefab;
            var buildingComponent = buildingPrefab.GetComponent<Building>();
            _buildingImage.sprite = buildingComponent.RenderSprite;
            _recipe = buildingComponent.Recipe;
        }

        public void UpdateCounter()
        {
            _counter.text = InventoryCanvas.InventoryContainer
                .GetRecipeMatchCount(_recipe).ToString();
            _buttonBlock.SetActive(_counter.text == "0");
        }

        public Tooltip GetTooltip()
        {
            TooltipElement title = new(BuildingPrefab.name, TooltipElement.FontType.Title);
            //TooltipElement description = new(BuildingPrefab.Description);
            //Tooltip thisTooltip = new(title, description);
            Tooltip thisTooltip = new(title);
            return thisTooltip.Merge(_recipe.GetTooltip());
        }
    }
}