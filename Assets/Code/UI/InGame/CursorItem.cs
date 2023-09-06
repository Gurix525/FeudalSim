using System;
using Input;
using Items;
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
        }

        private void OnEnable()
        {
            Cursor.ItemReferenceChanged.AddListener(OnCollectionUpdated);
        }

        private void OnDisable()
        {
            Cursor.ItemReferenceChanged.RemoveListener(OnCollectionUpdated);
        }

        private void Update()
        {
            transform.position = PlayerController.MainPoint.ReadValue<Vector2>();
        }

        private void OnCollectionUpdated(ItemReference item)
        {
            if (Cursor.ItemReference == null)
            {
                _text.text = string.Empty;
                _image.enabled = false;
                return;
            }
            //if (Cursor.Item.MaxStack == 1)
            //    _text.text = string.Empty;
            //else
            _text.text = Cursor.ItemReference.Item.Count.ToString();
            _image.sprite = Cursor.ItemReference.Item.Sprite;
            _image.enabled = true;
        }
    }
}