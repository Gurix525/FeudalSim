using System;
using UnityEngine;

namespace UI
{
    public class BuildingWindow : Window
    {
        [SerializeField] private Button _structuresButton;
        [SerializeField] private Button _furtnitureButton;
        [SerializeField] private Transform _structuresList;

        #region Public

        public void SwitchActive()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }

        #endregion Public

        #region Unity

        private void Awake()
        {
            _structuresButton.Clicked += _structuresButton_Clicked;
            _furtnitureButton.Clicked += _furtnitureButton_Clicked;
        }

        private void OnEnable()
        {
            LoadStructures();
        }

        #endregion Unity

        #region Private

        private void _structuresButton_Clicked(object sender, System.EventArgs e)
        {
            LoadStructures();
        }

        private void _furtnitureButton_Clicked(object sender, System.EventArgs e)
        {
            LoadFurniture();
        }

        private void LoadStructures()
        {

        }

        private void LoadFurniture()
        {

        }

        #endregion Private
    }
}