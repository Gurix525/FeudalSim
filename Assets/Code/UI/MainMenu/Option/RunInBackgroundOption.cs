using UnityEngine;

namespace UI
{
    public class RunInBackgroundOption : Option
    {
        protected override void ExecuteToggle(bool arg)
        {
            Application.runInBackground = arg;
        }
    }
}