using System;
using Maths;
using TMPro;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(LineRenderer))]
    public class AimCurve : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private int _nodesCount = 11;

        [SerializeField]
        private GameObject _marker;

        private LineRenderer _renderer;

        private Vector3 _markerPosition;
        private Vector3 _markerNormal;

        #endregion Fields

        #region Properties

        public BezierCurve Curve { get; private set; }

        public bool IsCurveEnabled { get; private set; }

        public Vector3[] Nodes => GetLineNodes();
        public int NodesCount => _nodesCount;

        #endregion Properties

        #region Public

        public Vector3 GetNodePosition(int index)
        {
            if (_renderer.positionCount <= index)
                return Vector3.zero;
            return _renderer.GetPosition(index);
        }

        public Vector3[] GetLineNodes()
        {
            Vector3[] nodes = new Vector3[NodesCount];
            _renderer.GetPositions(nodes);
            return nodes;
        }

        public void Enable()
        {
            IsCurveEnabled = true;
            _renderer.enabled = true;
            _marker.SetActive(true);
        }

        public void Disable()
        {
            IsCurveEnabled = false;
            _renderer.enabled = false;
            _marker.SetActive(false);
        }

        public void SetControlPoints(Vector3 start, Vector3 target, Vector3 targetNormal)
        {
            Curve = new(start, target, (start + target) / 2F
                + Vector3.up * Vector3.Distance(start, target) / 5F);
            _markerPosition = target;
            _markerNormal = targetNormal;
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
            if (!IsCurveEnabled)
                return;
            if (Curve == null)
                return;
            DrawLine();
            PlaceMarker();
            _renderer.material.SetVector("_StartPosition", Curve.StartPosition);
            _renderer.material.SetFloat("_CurveLength", Curve.ApproximateLength);
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
                nodes[i] = Curve.EvaluatePosition(t);
            }
            nodes = FindEndOfCurve(nodes);
            _renderer.positionCount = nodes.Length;
            _renderer.SetPositions(nodes);
        }

        private void PlaceMarker()
        {
            _marker.transform.position = _markerPosition + _markerNormal * 0.0001F;
            _marker.transform.LookAt(_markerPosition - _markerNormal);
        }

        private Vector3[] FindEndOfCurve(Vector3[] nodes)
        {
            for (int i = 0; i < nodes.Length - 1; i++)
            {
                if (Physics.Linecast(nodes[i], nodes[i + 1], out RaycastHit hit))
                {
                    _markerPosition = hit.point;
                    _markerNormal = hit.normal;
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