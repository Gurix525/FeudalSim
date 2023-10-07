using Controls;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ContainerSlot : MonoBehaviour, IMouseHandler
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _image;

        private int _slotIndex;
        private Container _container;

        #endregion Fields

        #region Public

        public void Initialize(int slotIndex, Container container)
        {
            _slotIndex = slotIndex;
            _container = container;
            _container.CollectionUpdated.AddListener(OnCollectionUpdated);
            OnCollectionUpdated();
        }

        public void Clear()
        {
            _container.CollectionUpdated.RemoveListener(OnCollectionUpdated);
        }

        #endregion Public

        #region Unity

        private void OnEnable()
        {
            if (_container != null)
            {
                OnCollectionUpdated();
                _container.CollectionUpdated.AddListener(OnCollectionUpdated);
            }
        }

        private void OnDisable()
        {
            _container.CollectionUpdated.RemoveListener(OnCollectionUpdated);
            // To be added
            //OnPointerExit(null);
        }

        #endregion Unity

        #region Private

        public void OnLeftMouseButton()
        {
            _container.OnLeftMouseButton(_slotIndex);
        }

        public void OnLeftMouseButtonRelase()
        {
            _container.OnLeftMouseButtonRelase(_slotIndex);
        }

        private void OnCollectionUpdated()
        {
            if (_container[_slotIndex] == null)
            {
                _text.text = string.Empty;
                _image.enabled = false;
                return;
            }
            //if (_container[_slotIndex].MaxStack == 1)
            //    _text.text = string.Empty;
            //else
            _text.text = _container[_slotIndex].Count.ToString();
            _image.sprite = _container[_slotIndex].Sprite;
            _image.enabled = true;
        }

        #endregion Private
    }
}