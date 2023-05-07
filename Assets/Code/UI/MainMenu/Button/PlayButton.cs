using UnityEngine;

namespace UI
{
    public class PlayButton : Button
    {
        [SerializeField] private GameObject _mainCanvas;
        [SerializeField] private GameObject _playCanvas;

        protected override void Execute()
        {
            _playCanvas.SetActive(true);
            _mainCanvas.SetActive(false);
        }
    }
}