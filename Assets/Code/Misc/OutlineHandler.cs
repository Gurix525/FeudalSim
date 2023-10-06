using Controls;
using UnityEngine;

namespace Misc
{
    [RequireComponent(typeof(Outline))]
    public class OutlineHandler : MonoBehaviour, IMouseHoverHandler
    {
        [SerializeField] private Texture2D _cursor;

        private Outline _outline;

        public void StartHover()
        {
            EnableOutline();
        }

        public void EndHover()
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

        private void EnableOutline()
        {
            _outline.enabled = true;
            if (_cursor != null)
                Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.Auto);
        }

        private void DisableOutline()
        {
            _outline.enabled = false;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }
    }
}