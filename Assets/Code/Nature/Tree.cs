using Controls;
using Items;
using Misc;
using UnityEngine;
using Cursor = Controls.Cursor;

namespace Nature
{
    [RequireComponent(typeof(OutlineHandler))]
    public class Tree : MonoBehaviour, ILeftClickHandler, IAxeActionOutline
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
            Item item = Item.Create("Wood", 0);
            Equipment.Insert(item);
            if (item.Count > 0)
                item.Drop(Player.Position);
            GameObject log = Instantiate(
                Resources.Load<GameObject>("Prefabs/Nature/Log"),
                transform.parent.parent);
            log.transform.position = transform.position;
            log.GetComponent<TemporaryRigidbody>().ActivateRigidbody(
                Vector3.forward * 100F,
                log.transform.position + Vector3.up * 10F,
                200F);
            Destroy(transform.parent.gameObject);
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