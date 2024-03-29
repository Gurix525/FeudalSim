using Cinemachine;
using Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Controls
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShake : MonoBehaviour
    {
        [SerializeField]
        private float _shakeDecay = 0.05F;

        private CinemachineBasicMultiChannelPerlin _cameraNoise;

        private static UnityEvent<float> CameraShaking { get; } = new();

        public static void ShakeCamera(float strength)
        {
            CameraShaking.Invoke(strength);
        }

        private void Awake()
        {
            _cameraNoise = GetComponent<CinemachineVirtualCamera>()
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            CameraShaking.AddListener(StartShakingCamera);
        }

        private void FixedUpdate()
        {
            _cameraNoise.m_AmplitudeGain -= _shakeDecay * Time.fixedDeltaTime;
            if (_cameraNoise.m_AmplitudeGain < 0.01F)
                _cameraNoise.m_AmplitudeGain = 0F;
        }

        private void StartShakingCamera(float strength)
        {
            strength = strength.Remap(0F, 100F, 0.3F, 3F);
            _cameraNoise.m_AmplitudeGain = Mathf.Max(_cameraNoise.m_AmplitudeGain, strength);
            _cameraNoise.m_AmplitudeGain = _cameraNoise.m_AmplitudeGain.Clamp(0F, 3F);
        }
    }
}