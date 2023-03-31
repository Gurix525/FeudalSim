using Input;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CursorItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private Image _image;
    private Container _container;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _container = Controls.Cursor.Container;
        _container.CollectionUpdated.AddListener(OnCollectionUpdated);
    }

    private void Update()
    {
        transform.position = PlayerController.MainPoint.ReadValue<Vector2>();
    }

    private void OnCollectionUpdated()
    {
        if (_container[0] == null)
            _text.text = string.Empty;
        else if (_container[0].MaxStack == 1)
            _text.text = string.Empty;
        else
            _text.text = _container[0].Count.ToString();
    }
}