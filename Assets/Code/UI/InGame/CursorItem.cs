using Items;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using PlayerCursor = Controls.PlayerCursor;

namespace UI
{
    public class CursorItem : MonoBehaviour
    {
        [SerializeField] private PlayerCursor _cursor;
        [SerializeField] private TextMeshProUGUI _text;

        private Image _image;

        private void OnMousePosition(InputValue value)
        {
            transform.position = value.Get<Vector2>();
        }

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void OnEnable()
        {
            _cursor.ItemReferenceChanged.AddListener(OnCollectionUpdated);
        }

        private void OnDisable()
        {
            _cursor.ItemReferenceChanged.RemoveListener(OnCollectionUpdated);
        }

        private void OnCollectionUpdated(ItemReference item)
        {
            if (_cursor.ItemReference == null)
            {
                _text.text = string.Empty;
                _image.enabled = false;
                return;
            }
            //if (Cursor.Item.MaxStack == 1)
            //    _text.text = string.Empty;
            //else
            _text.text = _cursor.ItemReference.Item.Count.ToString();
            _image.sprite = _cursor.ItemReference.Item.Sprite;
            _image.enabled = true;
        }
    }
}