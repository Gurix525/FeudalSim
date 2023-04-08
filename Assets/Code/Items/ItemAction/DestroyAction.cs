using UnityEngine;
using Controls;
using Cursor = Controls.Cursor;
using Buildings;

namespace Items
{
    public class DestroyAction : ItemAction
    {
        public override void Execute()
        {
            if (Cursor.RaycastHit == null)
                return;
            var building = Cursor.RaycastHit.Value.transform.GetComponent<Building>();
            if (building == null)
                return;
            GameObject.Destroy(building.gameObject);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}