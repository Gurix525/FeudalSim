using System;
using TMPro;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(LineRenderer))]
    public class AimCurve : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private int _nodesCount = 10;

        private LineRenderer _renderer;

        #endregion Fields

        #region Properties

        public Vector3 StartPosition { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Vector3 ControlPoint { get; set; }

        public int NodesCount => _nodesCount;

        #endregion Properties

        #region Public

        public Vector3 GetNodePosition(int index)
        {
            return _renderer.GetPosition(index);
        }

        public void Enable()
        {
            _renderer.enabled = true;
        }

        public void Disable()
        {
            _renderer.enabled = false;
        }

        public void SetControlPoints(Vector3 start, Vector3 target)
        {
            StartPosition = start;
            TargetPosition = target;
            ControlPoint = (start + target) / 2F
                + Vector3.up * Vector3.Distance(start, target) / 5F;
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();
            _renderer.material.renderQueue = 3001;
        }

        private void Update()
        {
            DrawLine();
            _renderer.material.SetVector("_StartPosition", StartPosition);
            _renderer.material.SetFloat("_CurveLength", GetLength());
        }

        #endregion Unity

        #region Private

        private void DrawLine()
        {
            if (_nodesCount < 2)
                return;
            Vector3[] nodes = new Vector3[_nodesCount];
            for (int i = 0; i < _nodesCount; i++)
            {
                float t = (float)i / (_nodesCount - 1);
                nodes[i] = GetBezierPoint(t);
            }
            nodes = FindEndOfCurve(nodes);
            _renderer.positionCount = nodes.Length;
            _renderer.SetPositions(nodes);
        }

        private Vector3 GetBezierPoint(float t)
        {
            return Mathf.Pow(1 - t, 2) * StartPosition
                + 2 * (1 - t) * t * ControlPoint
                + t * t * TargetPosition;
        }

        private float GetLength()
        {
            float length = 0F;
            Vector3[] nodes = new Vector3[NodesCount];
            _renderer.GetPositions(nodes);
            for (int i = 1; i < NodesCount; i++)
            {
                length += (nodes[i] - nodes[i - 1]).magnitude;
            }
            return length;
        }

        private Vector3[] FindEndOfCurve(Vector3[] nodes)
        {
            for (int i = 0; i < nodes.Length - 1; i++)
            {
                if (Physics.Linecast(nodes[i], nodes[i + 1], out RaycastHit hit))
                {
                    for (int j = i + 1; j < nodes.Length; j++)
                    {
                        nodes[j] = hit.point;
                    }
                    return nodes;
                }
            }
            return nodes;
        }

        #endregion Private
    }
}