using System;
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

        #endregion Properties

        #region Public

        public void Enable()
        {
            _renderer.enabled = true;
        }

        public void Disable()
        {
            _renderer.enabled = false;
        }

        public void SetControlPoints(Vector3 start, Vector3 target, Vector3 control)
        {
            StartPosition = start;
            TargetPosition = target;
            ControlPoint = control;
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _renderer = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            DrawLine();
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
            _renderer.positionCount = nodes.Length;
            _renderer.SetPositions(nodes);
        }

        private Vector3 GetBezierPoint(float t)
        {
            return Mathf.Pow(1 - t, 2) * StartPosition
                + 2 * (1 - t) * t * ControlPoint
                + t * t * TargetPosition;
        }

        #endregion Private
    }
}