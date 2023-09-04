using Controls;
using UnityEngine;
using Cursor = Controls.Cursor;
using PlayerControls;

namespace Misc
{
    [RequireComponent(typeof(Outline))]
    public class OutlineHandler : MonoBehaviour
    {
        private Outline _outline;
        private bool _isPointerOverGameObject;

        public void EnableOutline()
        {
            _outline.enabled = true;
        }

        public void DisableOutline()
        {
            _outline.enabled = false;
        }

        private void OnMouseOver()
        {
            if (Cursor.CurrentRaycastHit != null)
                if (Cursor.RaycastHit.Value.collider.gameObject == gameObject)
                {
                    EnableOutline();
                    return;
                }
            DisableOutline();
        }

        private void OnMouseExit()
        {
            DisableOutline();
        }

        private void Awake()
        {
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.OutlineColor = new(1F, 3F, 2F, 1F);
            _outline.OutlineWidth = 1F;
        }
    }
}