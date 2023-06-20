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

        public void EnableOutline()
        {
            if (Vector3.Distance(Player.Position, transform.position)
                <= CursorRaycaster.MaxCursorDistanceFromPlayer)
                _outline.enabled = true;
        }

        public void DisableOutline()
        {
            _outline.enabled = false;
        }

        private void Awake()
        {
            _outline = GetComponent<Outline>();
            _outline.enabled = false;
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.OutlineColor = new(1F, 3F, 2F, 1F);
            _outline.OutlineWidth = 0.5F;
        }
    }
}