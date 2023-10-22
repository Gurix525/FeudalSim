using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class LoadingImage : MonoBehaviour
    {
        public static LoadingImage Instance { get; private set; }

        private Image _image;
        private int _loadingCount = 0;

        public static void Enable()
        {
            Instance._loadingCount++;
        }

        public static void Disable()
        {
            Instance._loadingCount--;
            Instance._loadingCount = Instance._loadingCount < 0
                ? 0
                : Instance._loadingCount;
        }

        private void Awake()
        {
            Instance = this;
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            if (_loadingCount == 0)
                _image.enabled = false;
            else
                _image.enabled = true;
        }
    }
}