using Controls;
using UnityEngine;
using UnityEngine.UI;

public class TimerCircle : MonoBehaviour
{
    private Image _image;
    private float _currentTime = 0F;
    private float _requiredTime = 0F;
    private RectTransform _parent;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _image.type = Image.Type.Filled;
        _image.fillMethod = Image.FillMethod.Radial360;
        _image.fillClockwise = true;
        _image.fillAmount = 0F;
        _parent = transform.parent.GetComponent<RectTransform>();
        // To be added
        //ActionTimer.TimerSet.AddListener(StartMeasuring);
    }

    private void Update()
    {
        transform.localScale = (Vector2)_parent.localScale == Vector2.one * 0.5F
            ? Vector2.one * 2 : Vector2.one;
        _currentTime += Time.deltaTime;
        if (_requiredTime == 0F)
            _image.fillAmount = _image.fillAmount > 0.003F
                ? _image.fillAmount * 3F / 4F
                : 0F;
        else if (_image.fillAmount > _currentTime / _requiredTime)
            _image.fillAmount -= (_image.fillAmount - (_currentTime / _requiredTime)) / 4F;
        else
            _image.fillAmount = _currentTime / _requiredTime;
    }

    private void StartMeasuring(float requiredTime)
    {
        _currentTime = 0F;
        _requiredTime = requiredTime;
    }
}