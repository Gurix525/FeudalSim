using Buildings;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class BuildingButton : Button
    {
        [SerializeField] private Image _buildingImage;

        public GameObject BuildingPrefab { get; private set; }


        public void Initialize(GameObject buildingPrefab)
        {
            BuildingPrefab = buildingPrefab;
            _buildingImage.sprite = buildingPrefab.GetComponent<Building>().RenderSprite;
        }
    }
}