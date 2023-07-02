using System;
using Controls;
using Extensions;
using Items;
using UnityEngine;
using UnityEngine.EventSystems;
using Cursor = Controls.Cursor;
using Terrain = World.Terrain;

namespace Controls
{
    public class CursorMeshHighlight : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _notBlockedMaterial;
        [SerializeField] private Material _blockedMaterial;

        private GameObject _highlight;
        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private BuildingMode _buildingMode = BuildingMode.Floor;
        private Vector3 _previousPosition = Vector3.zero;
        private Mesh _previousMesh = null;
        private Material _previosMaterial = null;
        private BuildPositionFinder _positionFinder = new();

        private static float _meshRotation;

        #endregion Fields

        #region Properties

        public static bool IsBlocked { get; set; } = false;

        private static CursorMeshHighlight Instance { get; set; }

        private int RequiredItemCount => _buildingMode switch
        {
            BuildingMode.BigFloor => 4,
            BuildingMode.ShortWall => 1,
            BuildingMode.Wall => 2,
            BuildingMode.BigWall => 4,
            _ => 1
        };

        #endregion Properties

        #region Public

        public static void SetMesh(Mesh mesh)
        {
            if (Instance._mesh != mesh)
            {
                Instance._mesh = mesh;
                Instance._previousPosition = Vector3.zero;
            }
        }

        public static void SetBuildingMode(BuildingMode buildingMode)
        {
            Instance._buildingMode = buildingMode;
        }

        public static void SetMeshRotation(float rotation)
        {
            _meshRotation = rotation;
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            Instance = this;
            _highlight = new GameObject("MeshHighlight");
            _filter = _highlight.AddComponent<MeshFilter>();
            _renderer = _highlight.AddComponent<MeshRenderer>();
            _renderer.material = _notBlockedMaterial;
        }

        private void Update()
        {
            if (Cursor.Action is not BuildAction)
            {
                SetMesh(null);
            }
            if (_mesh != _previousMesh)
            {
                _filter.mesh = _mesh;
                _previousMesh = _mesh;
            }
            if (_previosMaterial != _notBlockedMaterial && !IsBlocked && Cursor.Item != null ? Cursor.Item.Count >= RequiredItemCount : false)
            {
                _renderer.material = _notBlockedMaterial;
                _previosMaterial = _notBlockedMaterial;
            }
            else if (_previosMaterial != _blockedMaterial && (IsBlocked || (Cursor.Item != null ? Cursor.Item.Count < RequiredItemCount : false)))
            {
                _renderer.material = _blockedMaterial;
                _previosMaterial = _blockedMaterial;
            }
            if (_mesh != null && Cursor.RaycastHit != null)
            {
                _renderer.enabled = true;
                _renderer.material.renderQueue = 3001;
                var position = Cursor.RaycastHit.Value.point;
                var calibratedPosition = _positionFinder.GetPosition(0, _meshRotation, position, _buildingMode);
                _highlight.transform.SetPositionAndRotation(
                    calibratedPosition,
                    Quaternion.Euler(0, _meshRotation, 0));
                if (calibratedPosition != _previousPosition)
                {
                    _previousPosition = calibratedPosition;
                    bool isBuildingPossible = Terrain.IsBuildingPossible(
                        calibratedPosition.ToVector3Int(),
                        _buildingMode,
                        _meshRotation);
                    if (isBuildingPossible)
                        IsBlocked = false;
                    else
                        IsBlocked = true;
                }
            }
            else
                _renderer.enabled = false;
        }

        #endregion Unity

        #region Private

        private class BuildPositionFinder
        {
            public int Height { get; set; } = 0;
            public float Angle { get; set; }
            public Vector3 Position { get; set; } = Vector3.zero;
            public BuildingMode Mode { get; set; } = BuildingMode.Floor;

            public Vector3 GetPosition(int height, float angle, Vector3 position, BuildingMode mode)
            {
                Height = height;
                Angle = angle;
                Position = position;
                Mode = mode;
                return mode switch
                {
                    BuildingMode.Floor => GetFloorPosition(),
                    BuildingMode.BigFloor => GetBigFloorPosition(),
                    BuildingMode.ShortWall => GetShortWallPosition(),
                    BuildingMode.Wall => GetWallPosition(),
                    BuildingMode.BigWall => GetBigWallPosition(),
                    _ => throw new System.NotImplementedException(),
                };
            }

            private Vector3 GetFloorPosition()
            {
                return new Vector3(
                    Position.x.Floor(),
                    Height,
                    Position.z.Floor()
                    );
            }

            private Vector3 GetBigFloorPosition()
            {
                return new Vector3(
                    Position.x.Round() - 1,
                    Height,
                    Position.z.Round() - 1);
            }

            private Vector3 GetShortWallPosition()
            {
                if (Angle == 0F)
                    return new Vector3(Position.x.Floor(), Height, Position.z.Round());
                else
                    return new Vector3(Position.x.Round(), Height, Position.z.Floor());
            }

            private Vector3 GetWallPosition()
            {
                return GetShortWallPosition();
            }

            private Vector3 GetBigWallPosition()
            {
                if (Angle == 0F)
                    return new Vector3(Position.x.Round() - 1, Height, Position.z.Round());
                else
                    return new Vector3(Position.x.Round(), Height, Position.z.Round() - 1);
            }
        }

        #endregion Private
    }
}