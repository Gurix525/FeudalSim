using Input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cursor = Controls.Cursor;

namespace UI
{
    public class CursorItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
            Cursor.Container.CollectionUpdated.AddListener(OnCollectionUpdated);
        }

        private void Update()
        {
            transform.position = PlayerController.MainPoint.ReadValue<Vector2>();
        }

        private void OnCollectionUpdated()
        {
            if (Cursor.Item == null)
            {
                _text.text = string.Empty;
                _image.enabled = false;
                return;
            }
            _image.rectTransform.localScale = Cursor.IsItemFromHotbar
                ? Vector2.one / 2 : Vector2.one;
            if (Cursor.Item.MaxStack == 1)
                _text.text = string.Empty;
            else
                _text.text = !Cursor.IsItemFromHotbar
                    ? Cursor.Item.Count.ToString()
                    : string.Empty;
            _image.sprite = Cursor.Item.Sprite;
            _image.enabled = true;
        }
    }
}