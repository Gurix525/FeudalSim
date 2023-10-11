using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UI
{
    public class VSyncOption : Option
    {
        protected override void ExecuteToggle(bool arg)
        {
            QualitySettings.vSyncCount = arg ? 1 : 0;
        }
    }
}