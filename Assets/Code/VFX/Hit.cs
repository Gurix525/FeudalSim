using System.Collections;
using Misc;
using UnityEngine;

namespace VFX
{
    public class Hit : MonoBehaviour
    {
        private static Pool<Hit> _pool = new("Prefabs/VFX/Hit");

        public static void Spawn(Vector3 position)
        {
            Hit hit = _pool.Pull();
            hit.transform.position = position;
            hit.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            StartCoroutine(FadeOff());
        }

        private IEnumerator FadeOff()
        {
            yield return new WaitForSeconds(1F);
            _pool.Push(this);
        }
    }
}