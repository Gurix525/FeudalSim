using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Misc;
using UI;
using UnityEngine;
using UnityEngine.AI;
using World;

public class BuildNavmeshButton : Button
{
    [SerializeField] private GameObject _agentPrefab;

    protected override void Execute()
    {
        var t = DateTime.Now;
        TerrainRenderer.NavMeshSurface.BuildNavMesh();
        var t2 = DateTime.Now;
        Debug.Log((t2 - t).TotalSeconds);
        for (int i = 0; i < 100; i++)
        {
            GameObject agent = Instantiate(_agentPrefab);
        }
    }
}