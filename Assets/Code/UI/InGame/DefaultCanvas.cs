using UnityEngine;

namespace UI
{
    public class DefaultCanvas : MonoBehaviour
    {
        [SerializeField] private Canvases _canvases;

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
            if (e != "Inventory")
                foreach (Transform child in transform)
                    child.gameObject.SetActive(false);
        }
    }
}