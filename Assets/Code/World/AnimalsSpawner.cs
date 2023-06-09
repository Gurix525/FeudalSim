using System.Collections;
using Extensions;
using Misc;
using UnityEngine;
using UnityEngine.AI;

namespace World
{
    public class AnimalsSpawner : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(SpawnAnimals());
        }

        private IEnumerator SpawnAnimals()
        {
            yield return new WaitUntil(() => NavMesh.SamplePosition(new Vector3(50F, 0F, 50F), out NavMeshHit hit, 10F, NavMesh.AllAreas));
            yield return 10;
            System.Random random = new();
            var prefabs = Resources.LoadAll<GameObject>("Prefabs/Animals");
            for (int i = 0; i < 0; i++)
            {
                NavMesh.SamplePosition(new RandomVector3(50F, 0F, 50F), out NavMeshHit hit, 10F, NavMesh.AllAreas);
                Instantiate(prefabs[0], hit.position, Quaternion.Euler(0F, random.NextFloat(0F, 360F), 0F), transform);
            }
            for (int i = 0; i < 20; i++)
            {
                NavMesh.SamplePosition(new RandomVector3(50F, 0F, 50F), out NavMeshHit hit, 10F, NavMesh.AllAreas);
                Instantiate(prefabs[1], hit.position, Quaternion.Euler(0F, random.NextFloat(0F, 360F), 0F), transform);
            }
        }
    }
}