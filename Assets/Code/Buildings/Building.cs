using Items;
using UnityEngine;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        private Item _backingItem;

        public void SetBackingItem(Item item)
        {
            _backingItem = item;
        }
    }
}