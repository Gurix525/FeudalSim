using System.Collections;
using System.Collections.Generic;
using Extensions;
using Misc;
using UnityEngine;
using UnityEngine.AI;

namespace World
{
    public class AnimalsSpawner : MonoBehaviour
    {
        private List<GameObject> _wolves = new();
        private List<GameObject> _hares = new();

        private bool _isInitialized;

        private void Start()
        {
            StartCoroutine(SpawnAnimals());
        }

        private void FixedUpdate()
        {
            if (!_isInitialized)
                return;
            System.Random random = new();
            for (int i = 0; i < _wolves.Count; i++)
            {
                var wolf = _wolves[i];
                if (wolf != null)
                {
                    if ((PlayerControls.Player.Position - wolf.transform.position).magnitude > 100F)
                        Destroy(wolf);
                }
                if (wolf == null)
                {
                    NavMesh.SamplePosition(PlayerControls.Player.Position + new RandomVector3(50F, 0F, 50F), out NavMeshHit hit, 100F, NavMesh.AllAreas);
                    wolf = Instantiate(Resources.Load<GameObject>("Prefabs/Animals/Wolf"), hit.position, Quaternion.Euler(0F, random.NextFloat(0F, 360F), 0F), transform);
                    _wolves[i] = wolf;
                }
            }
            for (int i = 0; i < _hares.Count; i++)
            {
                var hare = _hares[i];
                if (hare != null)
                {
                    if ((PlayerControls.Player.Position - hare.transform.position).magnitude > 100F)
                        Destroy(hare);
                }
                if (hare == null)
                {
                    NavMesh.SamplePosition(PlayerControls.Player.Position + new RandomVector3(50F, 0F, 50F), out NavMeshHit hit, 100F, NavMesh.AllAreas);
                    hare = Instantiate(Resources.Load<GameObject>("Prefabs/Animals/Hare"), hit.position, Quaternion.Euler(0F, random.NextFloat(0F, 360F), 0F), transform);
                    _hares[i] = hare;
                }
            }
        }

        private IEnumerator SpawnAnimals()
        {
            yield return new WaitUntil(() => NavMesh.SamplePosition(new Vector3(0F, 0F, 0F), out NavMeshHit hit, 10F, NavMesh.AllAreas));
            yield return 10;
            System.Random random = new();
            var prefabs = Resources.LoadAll<GameObject>("Prefabs/Animals");
            for (int i = 0; i < 15; i++)
            {
                NavMesh.SamplePosition(new RandomVector3(50F, 0F, 50F), out NavMeshHit hit, 10F, NavMesh.AllAreas);
                var hare = Instantiate(prefabs[0], hit.position, Quaternion.Euler(0F, random.NextFloat(0F, 360F), 0F), transform);
                _hares.Add(hare);
            }
            for (int i = 0; i < 15; i++)
            {
                NavMesh.SamplePosition(new RandomVector3(50F, 0F, 50F), out NavMeshHit hit, 10F, NavMesh.AllAreas);
                var wolf = Instantiate(prefabs[1], hit.position, Quaternion.Euler(0F, random.NextFloat(0F, 360F), 0F), transform);
                _wolves.Add(wolf);
            }
            _isInitialized = true;
        }
    }
}