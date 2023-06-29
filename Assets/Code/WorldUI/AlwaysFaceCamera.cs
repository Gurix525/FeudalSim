using UnityEngine;

namespace WorldUI
{
    [ExecuteInEditMode]
    public class AlwaysFaceCamera : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}