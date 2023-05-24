using AI;
using Controls;
using Items;
using Misc;
using UnityEngine;
using World;
using Cursor = Controls.Cursor;

namespace Nature
{
    [RequireComponent(typeof(OutlineHandler))]
    public class Boulder : MonoBehaviour, ILeftClickHandler, IPickaxeActionOutline, IDetectable
    {
        #region Fields

        private OutlineHandler _outlineHandler;

        #endregion Fields

        #region Public

        public void EnableOutline()
        {
            _outlineHandler.EnableOutline();
        }

        public void DisableOutline()
        {
            _outlineHandler.DisableOutline();
        }

        public void OnLeftMouseButton()
        {
            if (this == null)
                return;
            Item item = Item.Create("Stone", 0);
            Equipment.Insert(item);
            if (item.Count > 0)
                item.Drop(Player.Position);
            Destroy(gameObject);
            TerrainRenderer.MarkNavMeshToReload();
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _outlineHandler = GetComponent<OutlineHandler>();
        }

        private void OnMouseOver()
        {
            Cursor.Action.OnMouseOver(this);
        }

        private void OnMouseExit()
        {
            Cursor.Action.OnMouseExit(this);
        }

        #endregion Unity
    }
}