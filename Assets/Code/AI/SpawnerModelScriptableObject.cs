using System.Collections.Generic;
using AI;
using UnityEngine;

namespace Assets.Code.AI
{
    [CreateAssetMenu(fileName = "SpawnerModel", menuName = "Scriptable Objects/Spawner Model")]
    public class SpawnerModelScriptableObject : ScriptableObject
    {
        [field: SerializeField] public List<SpawnerItem> Entities { get; set; }
    }
}