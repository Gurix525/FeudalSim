using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControls
{
    public class PlayerVFX : MonoBehaviour
    {
        [SerializeField] private GameObject _firstSlash;
        [SerializeField] private GameObject _secondsSlash;

        public GameObject FirstSlash => _firstSlash;
        public GameObject SecondSlash => _secondsSlash;
    }
}