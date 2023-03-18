using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineActivator : MonoBehaviour
{
    private Outline _outline;

    private void OnMouseEnter()
    {
        if (_outline == null)
        {
            _outline = gameObject.AddComponent<Outline>();
            _outline.OutlineMode = Outline.Mode.OutlineVisible;
            _outline.OutlineColor = new Vector4(0.75F, 1.5F, 1.5F, 1F);
            _outline.OutlineWidth = 2F;
        }
        _outline.enabled = true;
    }

    private void OnMouseExit()
    {
        _outline.enabled = false;
    }
}