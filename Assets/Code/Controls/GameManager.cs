using System;
using UnityEngine;

namespace Controls
{
    public class GameManager : MonoBehaviour
    {
        public static string WorldName { get; set; }
        public static DateTime WorldCreationTime { get; set; }
        public static DateTime FullTimeInWorld { get; set; }
        public static DateTime LastPlayedTime { get; set; }

        private void FixedUpdate()
        {
            FullTimeInWorld.AddSeconds((double)Time.fixedDeltaTime);
        }
    }
}