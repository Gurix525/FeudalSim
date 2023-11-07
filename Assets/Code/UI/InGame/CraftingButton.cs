using Buildings;
using Controls;
using Items;
using UnityEngine;
using UnityEngine.UI;

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
    }
}