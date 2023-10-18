using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class BuildingCanvas : MonoBehaviour
    {
        [SerializeField] private BuildingWindow _buildingWindow;

        #region Input

        private void OnBuilding(InputValue value)
        {
            _buildingWindow.SwitchActive();
        }

        #endregion
    }
}