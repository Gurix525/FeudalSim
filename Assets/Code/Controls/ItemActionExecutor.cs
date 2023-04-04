using Input;
using TMPro;
using UnityEngine;
using World;
using static UnityEngine.InputSystem.InputAction;

namespace Controls
{
    public class ItemActionExecutor : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _modeText;

        private float _delta = -1F;
        private int _mode = 0;

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
                switch (_mode)
                {
                    case 0:
                        break;
                }
            if (_isShovelActive)
                switch (_mode)
                {
                    case 0:
                    case 1:
                        ModifyTerrainHeight();
                        break;

                    case 2:
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
            _mode++;
            if (_mode == 4)
                _mode = 0;
            _modeText.text = _mode switch
            {
                0 => $"Tryb: kopanie",
                1 => $"Tryb: wznoszenie",
                2 => $"Tryb: ścieżkowanie",
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