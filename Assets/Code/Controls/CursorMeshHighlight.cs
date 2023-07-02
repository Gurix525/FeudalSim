using System;
using Controls;
using Extensions;
using Input;
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
        private Vector3Int _previousPosition = Vector3Int.zero;
        private Mesh _previousMesh = null;
        private Material _previosMaterial = null;
        private BuildPositionFinder _positionFinder = new();

        private static float _meshRotation;
        private static int _height;

        #endregion Fields

        #region Properties

        public static bool IsBlocked { get; set; }

        public static int Height
        {
            get => _height;
            set
            {
                _height = Math.Clamp(value, -2, 15);
            }
        }

        public static Vector3Int Position { get; private set; }
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
                Position = Vector3Int.zero;
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
                var ray = Camera.main.ScreenPointToRay(PlayerController.MainPoint.ReadValue<Vector2>());
                new Plane(Vector3.up, Height).Raycast(ray, out float distance);
                var position = ray.origin + ray.direction * distance;
                var calibratedPosition = _positionFinder.GetPosition(Height, _meshRotation, position, _buildingMode);
                _highlight.transform.SetPositionAndRotation(
                    calibratedPosition,
                    Quaternion.Euler(0, _meshRotation, 0));
                if (calibratedPosition != Position)
                {
                    Position = calibratedPosition;
                    bool isBuildingPossible = Terrain.IsBuildingPossible(
                        calibratedPosition,
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
            public int Height { get; set; }
            public float Angle { get; set; }
            public Vector3 Position { get; set; }
            public BuildingMode Mode { get; set; }

            public Vector3Int GetPosition(int height, float angle, Vector3 position, BuildingMode mode)
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

            private Vector3Int GetFloorPosition()
            {
                return new Vector3(
                    Position.x.Floor(),
                    Height,
                    Position.z.Floor()
                    ).RoundToVector3Int();
            }

            private Vector3Int GetBigFloorPosition()
            {
                return new Vector3(
                    Position.x.Round() - 1,
                    Height,
                    Position.z.Round() - 1)
                    .RoundToVector3Int();
            }

            private Vector3Int GetShortWallPosition()
            {
                if (Angle == 0F)
                    return new Vector3(Position.x.Floor(), Height, Position.z.Round())
                        .RoundToVector3Int();
                else
                    return new Vector3(Position.x.Round(), Height, Position.z.Floor())
                        .RoundToVector3Int();
            }

            private Vector3Int GetWallPosition()
            {
                return GetShortWallPosition();
            }

            private Vector3Int GetBigWallPosition()
            {
                if (Angle == 0F)
                    return new Vector3(Position.x.Round() - 1, Height, Position.z.Round())
                        .RoundToVector3Int();
                else
                    return new Vector3(Position.x.Round(), Height, Position.z.Round() - 1)
                        .RoundToVector3Int();
            }
        }

        #endregion Private
    }
}