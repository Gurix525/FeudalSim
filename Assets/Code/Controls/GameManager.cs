using System;
using UnityEngine;

namespace Controls
{
    public class GameManager : MonoBehaviour
    {
        public static string WorldName { get; set; }
        public static long WorldCreationTime { get; set; }
        public static long FullTimeInWorld { get; set; }
        public static long LastPlayedTime { get; set; }

        private void FixedUpdate()
        {
            FullTimeInWorld += (long)(Time.fixedDeltaTime * 10000000);
        }
    }
}