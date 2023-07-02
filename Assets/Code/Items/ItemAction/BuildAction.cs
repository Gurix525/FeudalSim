using static UnityEngine.InputSystem.InputAction;
using Cursor = Controls.Cursor;
using Terrain = World.Terrain;
using UnityEngine;
using Extensions;
using Buildings;
using Controls;
using Input;
using World;

namespace Items
{
    public class BuildAction : ItemAction
    {
        #region Fields

        private BuildingMode _buildingMode = BuildingMode.Floor;
        private float _meshRotation;
        private GameObject[] _buildingPrefabs;
        private Vector3Int _calibratedPosition;
        private bool _isWaitingForAnotherBuilding = false;

        #endregion Fields

        #region Properties

        private GameObject[] BuildingPrefabs => _buildingPrefabs ??= new GameObject[5]
        {
            Resources.Load<GameObject>("Prefabs/Buildings/Floor"),
            Resources.Load<GameObject>("Prefabs/Buildings/BigFloor"),
            Resources.Load<GameObject>("Prefabs/Buildings/ShortWall"),
            Resources.Load<GameObject>("Prefabs/Buildings/Wall"),
            Resources.Load<GameObject>("Prefabs/Buildings/BigWall")
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

        public void SetBuildingMode(int modeNumber)
        {
            _buildingMode = (BuildingMode)modeNumber;
            if (Cursor.Action != this)
                return;
            ReloadCursorMeshHighlight();
        }

        public void ReloadCursorMeshHighlight()
        {
            CursorMeshHighlight.SetBuildingMode(_buildingMode);
            ResetRotationIfModeIsFloor();
        }

        public override void OnLeftMouseButton()
        {
            if (Cursor.CurrentRaycastHit == null)
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
            if (_isWaitingForAnotherBuilding)
            {
                DisableWaiting(new());
            }
            _ = ActionTimer.Start(FinishExecution, 1F);
        }

        public override void Update()
        {
            CursorMeshHighlight.SetMesh(_buildingMode switch
            {
                BuildingMode.Floor => Cursor.Item.BuildingMeshes[0],
                BuildingMode.BigFloor => Cursor.Item.BuildingMeshes[1],
                BuildingMode.ShortWall => Cursor.Item.BuildingMeshes[2],
                BuildingMode.Wall => Cursor.Item.BuildingMeshes[3],
                _ => Cursor.Item.BuildingMeshes[4]
            });
            CursorMeshHighlight.SetMeshRotation(_meshRotation);
            if (_isWaitingForAnotherBuilding)
            {
                OnLeftMouseButton();
            }
        }

        #endregion Public

        #region Protected

        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/" + _buildingMode switch
            {
                BuildingMode.Floor => "BuildActionFloor",
                BuildingMode.BigFloor => "BuildActionBigFloor",
                BuildingMode.ShortWall => "BuildActionShortWall",
                BuildingMode.Wall => "BuildActionWall",
                BuildingMode.BigWall => "BuildActionBigWall",
                _ => "Placeholder",
            });
        }

        #endregion Protected

        #region Private

        private void FinishExecution()
        {
            if (Cursor.CurrentRaycastHit == null)
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
            GameObject prefab = BuildingPrefabs[(int)_buildingMode];
            GameObject building = GameObject.Instantiate(
                prefab, TerrainRenderer.GetChunkRenderer(_calibratedPosition).Buildings);
            building.transform.SetPositionAndRotation(
                _calibratedPosition,
                Quaternion.Euler(0, _meshRotation, 0));
            Item inputItem = Cursor.Container.ExtractAt(0, RequiredItemCount);
            if (inputItem == null)
            {
                inputItem = Cursor.Item.Clone(RequiredItemCount);
                Cursor.Item.Count -= RequiredItemCount;
                Equipment.ClearEmptyItems();
            }
            building.GetComponent<Building>().
                Initialize(inputItem, _buildingMode);
            Terrain.SetBuildingMark(_calibratedPosition, _buildingMode, _meshRotation, true);
            TerrainRenderer.MarkNavMeshToReload();
            _isWaitingForAnotherBuilding = true;
            PlayerController.MainLeftClick.AddListener(ActionType.Canceled, DisableWaiting);
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
            if (Cursor.Action != this)
            {
                PlayerController.MainChange.RemoveListener(ActionType.Started, ChangeRotation);
                return;
            }
            PlayerController.MainChange.AddListener(ActionType.Started, ChangeRotation);
        }

        private void DisableWaiting(CallbackContext context)
        {
            _isWaitingForAnotherBuilding = false;
            PlayerController.MainLeftClick.RemoveListener(ActionType.Canceled, DisableWaiting);
        }

        #endregion Private
    }
}