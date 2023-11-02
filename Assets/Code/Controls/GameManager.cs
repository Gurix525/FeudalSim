using Buildings;
using Dialogues;
using Items;
using UnityEngine;

namespace Controls
{
    public class GameManager : MonoBehaviour
    {
        public static string WorldName { get; set; }
        public static long WorldCreationTime { get; set; }
        public static long FullTimeInWorld { get; set; }
        public static long LastPlayedTime { get; set; }

        private void Start()
        {
            LoadResources();
        }

        private void FixedUpdate()
        {
            FullTimeInWorld += (long)(Time.fixedDeltaTime * 10000000);
        }

        private void LoadResources()
        {
            Item.LoadResources();
            Building.LoadResources();
            Dialogue.LoadResources();
        }
    }
}