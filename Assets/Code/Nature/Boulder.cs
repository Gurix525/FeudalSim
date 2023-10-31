using Controls;
using Items;
using Misc;
using PlayerControls;
using UnityEngine;
using World;

namespace Nature
{
    [RequireComponent(typeof(OutlineHandler))]
    public class Boulder : MonoBehaviour, IMouseHandler
    {
        #region Public

        public void OnLeftMouseButton(Vector2 position)
        {
            if (this == null)
                return;
            Item item = Item.Create("Stone", 5);
            InventoryCanvas.Insert(item);
            if (item.Count > 0)
                item.Drop(Player.Position);
            Destroy(gameObject);
            Player.Instance.Stats.AddSkill("Digging", 1F);
            TerrainRenderer.MarkNavMeshToReload();
        }

        #endregion Public
    }
}