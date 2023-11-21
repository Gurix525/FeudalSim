using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private static bool _isActive = false;
        private static bool _hasToStop = false;
        private static Task _loadingTask;
        private static string _overwriteText = string.Empty;

        private static LoadingScreen _instance;

        public static void OverwriteText(string text)
        {
            _overwriteText = text;
        }

        public static void Clear()
        {
            _overwriteText = string.Empty;
            _instance._text.text = "Loading";
        }

        public static void Enable()
        {
            _instance.gameObject.SetActive(true);
            _hasToStop = false;
            if (_isActive == false)
                _loadingTask = Show();
        }

        public static void Disable()
        {
            _instance.gameObject.SetActive(false);
            _hasToStop = true;
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
                Destroy(gameObject);
            GetComponent<Canvas>().sortingOrder = 1000;
            gameObject.SetActive(false);
        }

        private static async Task Show()
        {
            int i = 0;
            _isActive = true;
            while (!_hasToStop)
            {
                if (_overwriteText.Length != 0)
                    _instance._text.text = _overwriteText;
                else
                    _instance._text.text = "Loading" + GetDots(i++);
                await Task.Yield();
            }
            _isActive = false;
        }

        private static string GetDots(int i)
        {
            i /= 20;
            string dots = "";
            for (int j = 0; j < i % 5; j++)
            {
                dots += ".";
            }
            return dots;
        }
    }
}