using System;
 
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlayerCursor = Controls.PlayerCursor;

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
            PlayerCursor.ItemReferenceChanged.AddListener(OnCollectionUpdated);
        }

        private void OnDisable()
        {
            PlayerCursor.ItemReferenceChanged.RemoveListener(OnCollectionUpdated);
        }

        private void Update()
        {
            // To be added
            //transform.position = PlayerController.MainPoint.ReadValue<Vector2>();
        }

        private void OnCollectionUpdated(ItemReference item)
        {
            if (PlayerCursor.ItemReference == null)
            {
                _text.text = string.Empty;
                _image.enabled = false;
                return;
            }
            //if (Cursor.Item.MaxStack == 1)
            //    _text.text = string.Empty;
            //else
            _text.text = PlayerCursor.ItemReference.Item.Count.ToString();
            _image.sprite = PlayerCursor.ItemReference.Item.Sprite;
            _image.enabled = true;
        }
    }
}