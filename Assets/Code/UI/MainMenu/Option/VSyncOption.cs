using UnityEngine;

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