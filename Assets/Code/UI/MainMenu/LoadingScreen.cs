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

        private static LoadingScreen _instance;

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
            GetComponent<Canvas>().sortingOrder = 1000;
            gameObject.SetActive(false);
        }

        private static async Task Show()
        {
            int i = 0;
            _isActive = true;
            while (!_hasToStop)
            {
                _instance._text.text = "Loading" + GetDots(i++);
                await Task.Delay(250);
            }
            _isActive = false;
        }

        private static string GetDots(int i)
        {
            string dots = "";
            for (int j = 0; j < i % 5; j++)
            {
                dots += ".";
            }
            return dots;
        }
    }
}