using System;
using Buildings;
using Controls;
using PlayerControls;
using UnityEngine;

namespace UI
{
    public class BuildingWindow : Window
    {
        #region Fields

        [SerializeField] private RightHandItemHook _rightHandItemHook;
        [SerializeField] private Button _structuresButton;
        [SerializeField] private Button _furtnitureButton;
        [SerializeField] private Button _destroyingButton;
        [SerializeField] private Transform _structuresList;
        [SerializeField] private GameObject _destroyingImage;

        private GameObject _buildingButtonPrefab;
        private BuildingMode _buildingMode;

        #endregion Fields

        #region Unity

        private void Awake()
        {
            _buildingButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/BuildingButton");
            _structuresButton.Clicked += _structuresButton_Clicked;
            _furtnitureButton.Clicked += _furtnitureButton_Clicked;
            _destroyingButton.Clicked += _destroyingButton_Clicked;
        }

        private void OnEnable()
        {
            if (BuildingCursor.Current != null)
                BuildingCursor.Current.gameObject.SetActive(true);
            if (_rightHandItemHook != null)
                _rightHandItemHook.SetItemActive("Hammer", true);
            Action methodToCall = _buildingMode switch
            {
                BuildingMode.Furniture => ActivateFurnitureMode,
                BuildingMode.Destroying => ActivateDestryingMode,
                _ => ActivateStructuresMode,
            };
            methodToCall();
        }

        private void OnDisable()
        {
            if (BuildingCursor.Current != null)
                BuildingCursor.Current.gameObject.SetActive(false);
            if (_rightHandItemHook != null)
                _rightHandItemHook.SetItemActive("Hammer", false);
        }

        #endregion Unity

        #region Private

        private void _structuresButton_Clicked(object sender, EventArgs e)
        {
            ActivateStructuresMode();
        }

        private void _furtnitureButton_Clicked(object sender, EventArgs e)
        {
            ActivateFurnitureMode();
        }

        private void _destroyingButton_Clicked(object sender, EventArgs e)
        {
            ActivateDestryingMode();
        }

        private void ActivateStructuresMode()
        {
            _buildingMode = BuildingMode.Structures;
            LoadBuildings(Building.Structures);
            BuildingCursor.Current.IsDestroying = false;
            _destroyingImage.SetActive(false);
        }

        private void ActivateFurnitureMode()
        {
            _buildingMode = BuildingMode.Furniture;
            LoadBuildings(Building.Furniture);
            BuildingCursor.Current.IsDestroying = false;
            _destroyingImage.SetActive(false);
        }

        private void ActivateDestryingMode()
        {
            _buildingMode = BuildingMode.Destroying;
            ClearButtons();
            BuildingCursor.Current.BuildingPrefab = null;
            BuildingCursor.Current.IsDestroying = true;
            _destroyingImage.SetActive(true);
        }

        private void LoadBuildings(GameObject[] buildings)
        {
            ClearButtons();
            foreach (var building in buildings)
            {
                GameObject button = Instantiate(_buildingButtonPrefab, _structuresList);
                button.GetComponent<BuildingButton>().Initialize(building);
            }
        }

        private void ClearButtons()
        {
            for (int i = 0; i < _structuresList.childCount; i++)
            {
                Destroy(_structuresList.GetChild(i).gameObject);
            }
        }

        #endregion Private

        private enum BuildingMode
        {
            Structures,
            Furniture,
            Destroying
        }
    }
}