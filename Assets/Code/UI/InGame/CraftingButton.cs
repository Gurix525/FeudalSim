using Buildings;
using Controls;
using Items;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace UI
{
    public class CraftingButton : Button
    {
        [SerializeField] private Image _itemImage;

        public ItemModel ItemModel { get; private set; }

        protected override void Execute()
        {
            base.Execute();
            // To be added
        }

        public void Initialize(ItemModel itemModel)
        {
            ItemModel = itemModel;
            _itemImage.sprite = itemModel.Sprite;
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