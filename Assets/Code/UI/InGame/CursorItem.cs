using System;
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
            //if (Cursor.Item.MaxStack == 1)
            //    _text.text = string.Empty;
            //else
            _text.text = Cursor.Item.Count.ToString();
            _image.sprite = Cursor.Item.Sprite;
            _image.enabled = true;
        }
    }
}