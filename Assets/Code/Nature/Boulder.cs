using AI;
using Controls;
using Items;
using Misc;
using UnityEngine;
using World;
using Cursor = Controls.PlayerCursor;
using PlayerControls;

namespace Nature
{
    [RequireComponent(typeof(OutlineHandler))]
    public class Boulder : MonoBehaviour, IMouseHandler
    {
        #region Public

        public void OnLeftMouseButton()
        {
            if (this == null)
                return;
            Item item = Item.Create("Stone", 5);
            Equipment.Insert(item);
            if (item.Count > 0)
                item.Drop(Player.Position);
            Destroy(gameObject);
            Player.Instance.Stats.AddSkill("Digging", 1F);
            TerrainRenderer.MarkNavMeshToReload();
        }

        #endregion Public
    }
}