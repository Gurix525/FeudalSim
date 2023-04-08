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

        public override void OnMouseEnter(Component component)
        {
            (component as Building)?.ChangeColor(new(1F, 0.75F, 0.75F));
        }

        public override void OnMouseExit(Component component)
        {
            (component as Building)?.ResetColor();
        }
    }
}