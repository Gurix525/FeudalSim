using UnityEngine;

namespace UI
{
    public abstract class Window : MonoBehaviour
    {
        protected static Vector2 _originalOffset = new(0F, 160F);
        protected RectTransform _rectTransform;
        public Vector2 CurrentOffset { get; set; }
    }
}