using UnityEngine;

namespace UI
{
    public class ChangeCanvasButton : Button
    {
        [SerializeField] private GameObject _nextCanvas;

        protected override void Execute()
        {
            _nextCanvas.SetActive(true);
            GetComponentInParent<Canvas>().gameObject.SetActive(false);
        }
    }
}