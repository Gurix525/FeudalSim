using System.Collections.Generic;
using Assets.Code.AI;
using UnityEngine;

namespace AI
{
    public class Spawner : MonoBehaviour
    {
        private List<SpawnerItem> _entities = new();

        private List<GameObject> _aliveEntities = new();
        private Transform _entitiesParent;

        private void Start()
        {
            _entitiesParent = GameObject.Find("Entities").transform;
        }

        public void Initialize(SpawnerModelScriptableObject model)
        {
            _entities = model.Entities;
        }

        public void SpawnAll()
        {
            foreach (SpawnerItem item in _entities)
            {
                for (int i = 0; i < item.EntitiesCount; i++)
                {
                    GameObject entity = Instantiate(item.EntityPrefab, transform.position, Quaternion.identity, _entitiesParent);
                    _aliveEntities.Add(entity);
                }
            }
        }

        public override string ToString()
        {
            return string.Join("\n", _entities);
        }
    }
}