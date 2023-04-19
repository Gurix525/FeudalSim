using Items;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace UI
{
    public class HotbarWindow : Window
    {
        private GameObject[] _slots;

        private void Start()
        {
            for (int i = 0; i < 8; i++)
            {
                _slots = new GameObject[8];
                _slots[i] = Instantiate(
                    Resources.Load<GameObject>("Prefabs/UI/HotbarSlot"),
                    transform);
                var containerSlot = _slots[i].GetComponent<HotbarSlot>();
                containerSlot.Initialize(i, Equipment.InventoryContainer);
            }
        }
    }
}