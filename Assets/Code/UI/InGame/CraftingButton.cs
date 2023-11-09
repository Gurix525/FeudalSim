using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CraftingButton : Button
    {
        [SerializeField] private Image _itemImage;
        [SerializeField] private TextMeshProUGUI _counter;
        [SerializeField] private GameObject _buttonBlock;

        public ItemModel ItemModel { get; private set; }

        protected override void Execute()
        {
            base.Execute();
            CraftItem();
        }

        public void Initialize(ItemModel itemModel)
        {
            ItemModel = itemModel;
            _itemImage.sprite = itemModel.Sprite;
        }

        public void UpdateCounter()
        {
            _counter.text = InventoryCanvas.InventoryContainer
                .GetRecipeMatchCount(ItemModel.Recipe).ToString();
            _buttonBlock.SetActive(_counter.text == "0");
        }

        private void CraftItem()
        {
            if (!InventoryCanvas.InventoryContainer.MatchesRecipe(ItemModel.Recipe))
                return;
            InventoryCanvas.InventoryContainer.RemoveRecipeItems(ItemModel.Recipe);
            InventoryCanvas.InventoryContainer.Insert(Item.Create(ItemModel.Name));
        }
    }
}