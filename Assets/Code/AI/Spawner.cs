using System;
using System.Collections.Generic;
using Assets.Code.AI;
using UnityEngine;
using World;

namespace AI
{
    public class Spawner : MonoBehaviour
    {
        private List<SpawnerItem> _entities = new();

        private List<GameObject> _aliveEntities = new();
        private Transform _entitiesParent;

        public void Initialize(SpawnerModelScriptableObject model)
        {
            _entities = model.Entities;
        }

        public void SpawnAll()
        {
            _entitiesParent ??= transform.parent.parent.GetComponent<ChunkRenderer>().Entities;
            if (_entitiesParent == null)
                return;
            foreach (SpawnerItem item in _entities)
            {
                for (int i = 0; i < item.EntitiesCount; i++)
                {
                    GameObject entity = Instantiate(item.EntityPrefab, transform.position, Quaternion.identity, _entitiesParent);
                    _aliveEntities.Add(entity);
                    var entityComponent = entity.GetComponent<Entity>(); 
                    entityComponent.EntityDestroyed += Spawner_EntityDestroyed;
                    entityComponent.Initialize(this);
                }
            }
        }

        private void Spawner_EntityDestroyed(object sender, EventArgs e)
        {
            var entity = (Entity)sender;
            if (entity == null)
                return;
            if (entity.gameObject == null)
                return;
            _aliveEntities.Remove(entity.gameObject);
            if (_aliveEntities.Count == 0)
                SpawnAll();
        }

        public override string ToString()
        {
            return string.Join("\n", _entities);
        }
    }
}