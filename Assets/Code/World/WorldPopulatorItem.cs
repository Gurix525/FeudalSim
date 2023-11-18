using System;
using Assets.Code.AI;
using UnityEngine;

namespace World
{
    [Serializable]
    public class WorldPopulatorItem
    {
        [field: SerializeField] public SpawnerModelScriptableObject SpawnerModel { get; set; }
        [field: SerializeField][field: Range(0, 100)] public int Count { get; set; }
    }
}