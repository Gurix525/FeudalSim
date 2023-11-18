using System;
using UnityEngine;

namespace AI
{
    [Serializable]
    public class SpawnerItem
    {
        [field: SerializeField] public GameObject EntityPrefab;
        [field: SerializeField] public int EntitiesCount;

        public string Name => EntityPrefab.name;

        public override string ToString()
        {
            return $"{Name}: {EntitiesCount}";
        }
    }
}