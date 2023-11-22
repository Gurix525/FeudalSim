using UI;
using UnityEngine;

public class MapCanvas : MonoBehaviour
{
    [SerializeField] private Canvases _canvases;
    [SerializeField] private MapWindow _mapWindow;

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
        if (e == "Map")
            _mapWindow.SwitchActive();
        else
            _mapWindow.gameObject.SetActive(false);
    }
}