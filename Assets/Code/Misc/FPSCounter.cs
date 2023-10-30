using System.Collections;
using UnityEngine;

namespace Misc
{
    public class FPSCounter : MonoBehaviour
    {
        private TMPro.TextMeshProUGUI _text;
        private int counter = 0;

        private void Awake()
        {
            _text = GetComponent<TMPro.TextMeshProUGUI>();
        }

        private IEnumerator Start()
        {
            while (true)
            {
                _text.text = $"{counter * 5}";
                counter = 0;
                yield return new WaitForSeconds(0.2f);
            }
        }

        private void Update()
        {
            counter++;
        }
    }
}