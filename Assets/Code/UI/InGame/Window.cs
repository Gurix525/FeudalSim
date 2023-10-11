using UnityEngine;

namespace UI
{
    public abstract class Window : MonoBehaviour
    {
        protected static Vector2 _offset = new(0F, 160F);
        protected RectTransform _rectTransform;
    }
}