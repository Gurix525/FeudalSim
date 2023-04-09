using UnityEngine;
using Controls;
using Cursor = Controls.Cursor;
using Buildings;
using System.Threading.Tasks;

namespace Items
{
    public class DestroyAction : ItemAction
    {
        private Building _buildingToDestroy = null;

        public override void Execute()
        {
            if (Cursor.RaycastHit == null)
                return;
            _buildingToDestroy = Cursor.RaycastHit.Value.transform.GetComponent<Building>();
            if (_buildingToDestroy == null)
                return;
            _ = ActionTimer.Start(FinishExecution, 1F);
        }

        public override void OnMouseEnter(Component component)
        {
            (component as Building)?.ChangeColor(new(1F, 0.75F, 0.75F));
        }

        public override void OnMouseExit(Component component)
        {
            (component as Building)?.ResetColor();
        }

        private void FinishExecution()
        {
            if (Cursor.RaycastHit == null)
                return;
            Building buildingToDestroy = Cursor.RaycastHit.Value.transform.GetComponent<Building>();
            if (buildingToDestroy == null)
                buildingToDestroy = _buildingToDestroy;
            GameObject.Destroy(buildingToDestroy.gameObject);
        }
    }
}