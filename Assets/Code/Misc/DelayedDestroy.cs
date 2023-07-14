using System.Collections;
using UnityEngine;

namespace Misc
{
    public class DelayedDestroy : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(DestroyDelayed());
        }

        private IEnumerator DestroyDelayed()
        {
            yield return new WaitForSeconds(1F);
            Destroy(gameObject);
        }
    }
}