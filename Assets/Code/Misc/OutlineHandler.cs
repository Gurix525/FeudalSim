using UnityEngine;
using Cursor = Controls.Cursor;

namespace Misc
{
    [RequireComponent(typeof(Outline))]
    public class OutlineHandler : MonoBehaviour
    {
        [SerializeField] private Texture2D _cursor;

        private Outline _outline;
        private bool _isOutlineEnabled = false;

        public void EnableOutline()
        {
            _outline.enabled = true;
            _isOutlineEnabled = true;
            if (_cursor != null)
                UnityEngine.Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.Auto);
        }

        public void DisableOutline()
        {
            _outline.enabled = false;
            _isOutlineEnabled = false;
            UnityEngine.Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
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

        private void OnDisable()
        {
            DisableOutline();
        }

        private void OnDestroy()
        {
            DisableOutline();
        }
    }
}