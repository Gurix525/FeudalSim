using UnityEngine;

namespace UI
{
    public class BuildingCanvas : MonoBehaviour
    {
        [SerializeField] private Canvases _canvases;
        [SerializeField] private BuildingWindow _buildingWindow;

        private void OnEnable()
        {
            _canvases.CommandPassed += _canvases_CommandPassed;
        }

        private void OnDisable()
        {
            _canvases.CommandPassed -= _canvases_CommandPassed;
        }

        private void _canvases_CommandPassed(object sender, string e)
        {
            if (e == "Building")
                _buildingWindow.SwitchActive();
            else
                _buildingWindow.gameObject.SetActive(false);
        }
    }
}