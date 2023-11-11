using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TooltipElementView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _text;

        public void Initialize(TooltipElement element)
        {
            _image.sprite = element.Sprite;
            _text.fontSize = element.FontSize;
            _text.text = element.Text;
            if (element.Sprite == null)
            {
                _image.gameObject.SetActive(false);
                _text.rectTransform.sizeDelta = new(320F, 1F);
            }
        }
    }
}