using UnityEngine;
using UnityEngine.UI;

public class MenuBackground : MonoBehaviour
{
    private Image _image;
    private RectTransform _rectTransform;

    private static Sprite[] _sprites;

    private static Sprite _currentSprite;

    private void OnDestroy()
    {
        _currentSprite = null;
    }

    private void Awake()
    {
        System.Random random = new();
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _sprites ??= Resources.LoadAll<Sprite>("Images/Menu/Background");
        _currentSprite ??= _sprites[random.Next(0, _sprites.Length - 1)];
        _image.sprite = _currentSprite;
    }

    private void Update()
    {
        //_rectTransform.sizeDelta = new(Screen.width * 2, Screen.height * 2);
    }
}