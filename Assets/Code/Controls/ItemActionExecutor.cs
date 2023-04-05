using Input;
using TMPro;
using UnityEngine;
using World;
using static UnityEngine.InputSystem.InputAction;

namespace Controls
{
    public class ItemActionExecutor : MonoBehaviour
    {
        [SerializeField] private Mesh[] _meshes;
        [SerializeField] private TextMeshProUGUI _modeText;

        private float _delta = -1F;
        private BuildingMode _buildingMode = BuildingMode.Floor;
        private ShovelMode _shovelMode = ShovelMode.Digging;

        private static bool _isShovelActive = false;
        private static bool _isBuildingActive = true;

        private void OnEnable()
        {
            PlayerController.MainUse.AddListener(ActionType.Started, Execute);
            PlayerController.MainChange.AddListener(ActionType.Started, ChangeMode);
        }

        private void OnDisable()
        {
            PlayerController.MainUse.RemoveListener(ActionType.Started, Execute);
            PlayerController.MainChange.RemoveListener(ActionType.Started, ChangeMode);
        }

        private void Execute(CallbackContext context)
        {
            if (_isBuildingActive)
                switch (_buildingMode)
                {
                    default:
                        break;
                }
            if (_isShovelActive)
                switch (_shovelMode)
                {
                    case ShovelMode.Digging:
                    case ShovelMode.Rising:
                        ModifyTerrainHeight();
                        break;

                    case ShovelMode.Pathing:
                        Pathen();
                        break;

                    default:
                        Plow();
                        break;
                }
        }

        private void ChangeMode(CallbackContext context)
        {
            _delta *= -1F;
            _buildingMode = (int)(_buildingMode + 1) > 4 ? 0 : _buildingMode + 1;
            _shovelMode = (int)(_shovelMode + 1) > 3 ? 0 : _shovelMode + 1;
            _modeText.text = _shovelMode switch
            {
                ShovelMode.Digging => $"Tryb: kopanie",
                ShovelMode.Rising => $"Tryb: wznoszenie",
                ShovelMode.Pathing => $"Tryb: ścieżkowanie",
                _ => $"Tryb: oranie"
            };
        }

        private void Plow()
        {
            if (Cursor.CellPosition != null)
                World.Terrain.ChangeColor((Vector2Int)Cursor.CellPosition, Color.green);
        }

        private void Pathen()
        {
            if (Cursor.CellPosition != null)
                World.Terrain.ChangeColor((Vector2Int)Cursor.CellPosition, Color.red);
        }

        private void ModifyTerrainHeight()
        {
            if (Cursor.CellPosition != null)
                World.Terrain.ModifyHeight((Vector2Int)Cursor.CellPosition, _delta);
        }
    }
}