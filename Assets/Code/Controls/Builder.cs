using Buildings;
using Extensions;
using Input;
using Misc;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using Terrain = World.Terrain;

namespace Controls
{
    public class Builder : MonoBehaviour
    {
        #region Fields

        private BuildingMode _buildingMode = BuildingMode.BigWall;
        private float _meshRotation = 0F;
        private GameObject[] _buildingPrefabs;

        private GameObject[] BuildingPrefabs => _buildingPrefabs ??= new GameObject[5]
        {
            Prefabs.GetPrefab("Floor"),
            Prefabs.GetPrefab("BigFloor"),
            Prefabs.GetPrefab("ShortWall"),
            Prefabs.GetPrefab("Wall"),
            Prefabs.GetPrefab("BigWall")
        };

        #endregion Fields

        #region Unity

        private void OnEnable()
        {
            PlayerController.MainQuickMenu.AddListener(ActionType.Started, ChangeMode);
            PlayerController.MainChange.AddListener(ActionType.Started, ChangeRotation);
            PlayerController.MainUse.AddListener(ActionType.Started, Build);
        }

        private void Update()
        {
            if (Cursor.Item == null)
            {
                CursorMeshHighlight.TrySetMesh(null);
                return;
            }
            if (Cursor.Item.IsEligibleForBuilding)
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
        }

        private void OnDisable()
        {
            PlayerController.MainQuickMenu.RemoveListener(ActionType.Started, ChangeMode);
            PlayerController.MainChange.RemoveListener(ActionType.Started, ChangeRotation);
            PlayerController.MainUse.RemoveListener(ActionType.Started, Build);
        }

        #endregion Unity

        #region Private

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

        private void Build(CallbackContext context)
        {
            if (Cursor.Item == null || Cursor.ExactPosition == null)
                return;
            if (!Cursor.Item.IsEligibleForBuilding)
                return;
            var position = Cursor.ExactPosition.Value;
            var calibratedPosition = new Vector3(
                Mathf.Floor(position.x),
                Mathf.Round(position.y),
                Mathf.Floor(position.z)).ToVector3Int();
            if (!Terrain.IsBuildingPossible(calibratedPosition, _buildingMode, _meshRotation))
                return;
            GameObject prefab = BuildingPrefabs[(int)_buildingMode];
            GameObject building = Instantiate(prefab);
            building.transform.SetPositionAndRotation(
                calibratedPosition,
                Quaternion.Euler(0, _meshRotation, 0));
            building.GetComponent<MeshRenderer>().material = Cursor.Item.Material;
            building.GetComponent<Building>().SetBackingItem(Cursor.Item.Clone());
            Terrain.SetBuildingMark(calibratedPosition, _buildingMode, _meshRotation, true);
        }

        private void ResetRotationIfModeIsFloor()
        {
            if (_buildingMode == BuildingMode.Floor || _buildingMode == BuildingMode.BigFloor)
                _meshRotation = 0F;
        }

        #endregion Private
    }
}