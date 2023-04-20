using Input;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace UI
{
    public class SortButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Container _container;

        public void Initialize(Container container)
        {
            _container = container;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayerController.MainUse.AddListener(ActionType.Started, Sort);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayerController.MainUse.RemoveListener(ActionType.Started, Sort);
        }

        private void OnDisable()
        {
            OnPointerExit(null);
        }

        private void Sort(CallbackContext context)
        {
            _container.Sort();
        }
    }
}