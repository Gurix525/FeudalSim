using Buildings;
using Controls;
using UnityEngine;
using World;
using Cursor = Controls.Cursor;
using Misc;
using Extensions;
using Terrain = World.Terrain;
using System;
using static UnityEngine.InputSystem.InputAction;
using Input;

namespace Items
{
    public class BuildAction : ItemAction
    {
        #region Fields

        private BuildingMode _buildingMode = BuildingMode.Floor;
        private float _meshRotation = 0F;
        private GameObject[] _buildingPrefabs;
        private Vector3Int _calibratedPosition;

        #endregion Fields

        #region Properties

        private GameObject[] BuildingPrefabs => _buildingPrefabs ??= new GameObject[5]
        {
            Prefabs.GetPrefab("Floor"),
            Prefabs.GetPrefab("BigFloor"),
            Prefabs.GetPrefab("ShortWall"),
            Prefabs.GetPrefab("Wall"),
            Prefabs.GetPrefab("BigWall")
        };

        private int RequiredItemCount => _buildingMode switch
        {
            BuildingMode.BigFloor => 4,
            BuildingMode.ShortWall => 1,
            BuildingMode.Wall => 2,
            BuildingMode.BigWall => 4,
            _ => 1
        };

        #endregion Properties

        #region Constructors

        public BuildAction()
        {
            Cursor.Container.CollectionUpdated.AddListener(OnCursorCollectionUpdated);
        }

        #endregion Constructors

        #region Public

        public override void Execute()
        {
            if (Cursor.RaycastHit == null)
                return;
            if (Cursor.Item.Count < RequiredItemCount)
                return;
            var position = Cursor.RaycastHit.Value.point;
            _calibratedPosition = new Vector3(
                Mathf.Floor(position.x),
                Mathf.Round(position.y),
                Mathf.Floor(position.z)).ToVector3Int();
            if (!Terrain.IsBuildingPossible(_calibratedPosition, _buildingMode, _meshRotation))
                return;
            _ = ActionTimer.Start(FinishExecution, 1F);
        }

        public override void Update()
        {
            CursorMeshHighlight.TrySetMesh(_buildingMode switch
            {
                BuildingMode.Floor => Cursor.Item.BuildingMeshes[0],
                BuildingMode.BigFloor => Cursor.Item.BuildingMeshes[1],
                BuildingMode.ShortWall => Cursor.Item.BuildingMeshes[2],
                BuildingMode.Wall => Cursor.Item.BuildingMeshes[3],
                _ => Cursor.Item.BuildingMeshes[4]
            });
            CursorMeshHighlight.SetMeshRotation(_meshRotation);
        }

        #endregion Public

        #region Private

        private void FinishExecution()
        {
            GameObject prefab = BuildingPrefabs[(int)_buildingMode];
            GameObject building = GameObject.Instantiate(
                prefab, TerrainRenderer.GetChunkRenderer(_calibratedPosition).Buildings);
            building.transform.SetPositionAndRotation(
                _calibratedPosition,
                Quaternion.Euler(0, _meshRotation, 0));
            building.GetComponent<MeshRenderer>().material = Cursor.Item.Material;
            building.GetComponent<Building>().
                Initialize(Cursor.Container.ExtractAt(0, RequiredItemCount), _buildingMode);
            Terrain.SetBuildingMark(_calibratedPosition, _buildingMode, _meshRotation, true);
        }

        private void ChangeMode(CallbackContext context)
        {
            _buildingMode = (int)(_buildingMode + 1) > 4 ? 0 : _buildingMode + 1;
            CursorMeshHighlight.SetBuildingMode(_buildingMode);
            ResetRotationIfModeIsFloor();
        }

        private void ChangeRotation(CallbackContext context)
        {
            _meshRotation = _meshRotation == 0F ? -90F : 0F;
            ResetRotationIfModeIsFloor();
        }

        private void ResetRotationIfModeIsFloor()
        {
            if (_buildingMode == BuildingMode.Floor || _buildingMode == BuildingMode.BigFloor)
                _meshRotation = 0F;
        }

        private void OnCursorCollectionUpdated()
        {
            if (Cursor.Item == null)
            {
                PlayerController.MainQuickMenu.RemoveListener(ActionType.Started, ChangeMode);
                PlayerController.MainChange.RemoveListener(ActionType.Started, ChangeRotation);
                return;
            }
            if (Cursor.Item.Action != this)
            {
                PlayerController.MainQuickMenu.RemoveListener(ActionType.Started, ChangeMode);
                PlayerController.MainChange.RemoveListener(ActionType.Started, ChangeRotation);
                return;
            }
            PlayerController.MainQuickMenu.AddListener(ActionType.Started, ChangeMode);
            PlayerController.MainChange.AddListener(ActionType.Started, ChangeRotation);
        }

        #endregion Private
    }
}