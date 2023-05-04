using Controls;
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
            throw new System.NotImplementedException();
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