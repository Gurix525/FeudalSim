using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace UI
{
    public class SortButton : Button
    {
        private Container _container;

        public void Initialize(Container container)
        {
            _container = container;
        }

        protected override void Execute()
        {
            _container.Sort();
        }
    }
}