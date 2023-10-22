using UnityEngine.Rendering.Universal;

namespace UI
{
    public class DepthOfFieldOption : Option
    {
        protected override void ExecuteToggle(bool arg)
        {
            _settings.Profile.TryGet(out DepthOfField depthOfField);
            depthOfField.active = arg;
        }
    }
}