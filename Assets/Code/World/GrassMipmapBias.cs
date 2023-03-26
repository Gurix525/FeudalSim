using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class GrassMipmapBias : MonoBehaviour
    {
        [SerializeField] private Texture _texture;
        [SerializeField] private float _mipMapBias;

        private void Start()
        {
            _texture.mipMapBias = _mipMapBias;
        }
    }
}