using System;
using Combat;
using Items;
using UnityEngine;

namespace PlayerControls
{
    public class LeftHandItemHook : MonoBehaviour
    {
        private GameObject _assignedItem;

        private void Awake()
        {
            throw new NotImplementedException();
            //Controls.Cursor.ItemChanged.AddListener(ChangeAssignedItem);
        }

        private void ChangeAssignedItem(Item item)
        {
            Destroy(_assignedItem);
            if (item == null)
                return;
            if (item.WeaponPrefab == null)
                return;
            if (!item.WeaponPrefab.GetComponent<Weapon>().IsLeftHanded)
                return;
            _assignedItem = Instantiate(item.WeaponPrefab);
            _assignedItem.transform.SetParent(transform, true);
            _assignedItem.transform
                .SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
    }
}