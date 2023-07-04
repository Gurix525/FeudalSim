using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingGrid : MonoBehaviour
{
    [SerializeField] private Material _material;

    private LineRenderer[] _renderers;

    public void Enable()
    {
        foreach (var renderer in _renderers)
            renderer.gameObject.SetActive(true);
        _material.renderQueue = 3001;
    }

    public void Disable()
    {
        foreach (var renderer in _renderers)
            renderer.gameObject.SetActive(false);
    }

    public void SetMousePosition(Vector3 position)
    {
        _material.SetVector("_MousePosition", position);
    }

    private void Start()
    {
        _renderers = GetComponentsInChildren<LineRenderer>(true);
        foreach (var renderer in _renderers)
            renderer.sharedMaterial = _material;
    }
}