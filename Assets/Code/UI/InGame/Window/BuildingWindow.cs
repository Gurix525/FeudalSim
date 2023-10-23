﻿using System;
using Buildings;
using Controls;
using UnityEngine;

namespace UI
{
    public class BuildingWindow : Window
    {
        [SerializeField] private Button _structuresButton;
        [SerializeField] private Button _furtnitureButton;
        [SerializeField] private Transform _structuresList;

        private GameObject _buildingButtonPrefab;

        #region Public

        public void SwitchActive()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _buildingButtonPrefab = Resources.Load<GameObject>("Prefabs/UI/BuildingButton");
            _structuresButton.Clicked += _structuresButton_Clicked;
            _furtnitureButton.Clicked += _furtnitureButton_Clicked;
        }

        private void OnEnable()
        {
            LoadStructures();
            if (BuildingCursor.Current != null)
                BuildingCursor.Current.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            if (BuildingCursor.Current != null)
                BuildingCursor.Current.gameObject.SetActive(false);
        }

        #endregion Unity

        #region Private

        private void _structuresButton_Clicked(object sender, EventArgs e)
        {
            LoadStructures();
        }

        private void _furtnitureButton_Clicked(object sender, EventArgs e)
        {
            LoadFurniture();
        }

        private void LoadStructures()
        {
            ClearButtons();
            foreach (var structure in Building.Structures)
            {
                GameObject button = Instantiate(_buildingButtonPrefab, _structuresList);
                button.GetComponent<BuildingButton>().Initialize(structure);
            }
        }

        private void LoadFurniture()
        {
        }

        private void ClearButtons()
        {
            for (int i = 0; i < _structuresList.childCount; i++)
            {
                Destroy(_structuresList.GetChild(i).gameObject);
            }
        }

        #endregion Private
    }
}