using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace UI
{
    public class BloomOption : Option
    {
        protected override void ExecuteToggle(bool arg)
        {
            _settings.Profile.TryGet(out Bloom bloom);
            bloom.active = arg;
        }
    }
}