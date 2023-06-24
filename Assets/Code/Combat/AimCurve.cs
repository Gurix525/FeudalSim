using System;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(LineRenderer))]
    public class AimCurve : MonoBehaviour
    {
        [SerializeField]
        private GameObject P1, P2, P3;

        [SerializeField]
        private int _nodesCount = 10;

        private LineRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            DrawLine();
        }

        private void DrawLine()
        {
            if (_nodesCount < 2)
                return;
            Vector3[] nodes = new Vector3[_nodesCount];
            for (int i = 1; i < _nodesCount + 1; i++)
            {
                float t = 1 / (float)_nodesCount;
                nodes[i - 1] = GetBezierPoint(t);
            }
            _renderer.positionCount = nodes.Length;
            _renderer.SetPositions(nodes);
        }

        private Vector3 GetBezierPoint(float t)
        {
            Vector3 p0 = P1.transform.position;
            Vector3 p1 = P2.transform.position;
            Vector3 p2 = P3.transform.position;
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            Vector3 p = uu * p0;
            p += 2 * u * t * p1;
            p += tt * p2;
            return p;
        }
    }
}