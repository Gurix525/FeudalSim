using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Misc
{
    public class DepthOfFieldDistance : MonoBehaviour
    {
        [SerializeField] private GameObject _camera;
        [SerializeField] private GameObject _player;

        private VolumeProfile _profile;
        private DepthOfField _depthOfField;

        private void Awake()
        {
            _profile = GetComponent<Volume>().profile;
            _profile.TryGet(out _depthOfField);
        }

        private void Update()
        {
            float distance = (_camera.transform.position - _player.transform.position).magnitude;
            _depthOfField.focusDistance.value = distance;
        }
    }
}