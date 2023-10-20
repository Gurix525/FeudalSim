using Buildings;
using Controls;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingButton : Button
    {
        [SerializeField] private Image _buildingImage;

        public GameObject BuildingPrefab { get; private set; }

        protected override void Execute()
        {
            base.Execute();
            BuildingCursor.Current.BuildingPrefab = BuildingPrefab;
        }

        public void Initialize(GameObject buildingPrefab)
        {
            BuildingPrefab = buildingPrefab;
            _buildingImage.sprite = buildingPrefab.GetComponent<Building>().RenderSprite;
        }
    }
}