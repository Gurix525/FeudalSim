using UnityEngine;
using UnityEngine.Rendering;

namespace UI
{
    public class Settings : MonoBehaviour
    {
        [field: SerializeField] public VolumeProfile Profile { get; set; }

        private Option[] _options;

        private void Awake()
        {
            _options = GetComponentsInChildren<Option>();
            InitializeAllOptions();
        }

        private void InitializeAllOptions()
        {
            foreach (var option in _options)
                option.Initialize(this);
        }

        private void ExecuteAllOptions()
        {
            foreach (var option in _options)
                option.Execute();
        }
    }
}