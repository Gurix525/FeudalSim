using UnityEngine;
using UnityEngine.ProBuilder;

namespace Controls
{
    public class MeshHighlight : MonoBehaviour
    {
        [SerializeField] private PlayerCursor _cursor;

        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void OnEnable()
        {
            _cursor.ItemReferenceChanged += Cursor_ItemReferenceChanged;
            _cursor.WorldPositionChanged += Cursor_WorldPositionChanged;
        }

        private void OnDisable()
        {
            _cursor.ItemReferenceChanged -= Cursor_ItemReferenceChanged;
            _cursor.WorldPositionChanged -= Cursor_WorldPositionChanged;
        }

        private void Cursor_WorldPositionChanged(object sender, RaycastHitChangedEventArgs e)
        {
            if (e.NewRaycastHit != null)
            {
                transform.position = e.NewRaycastHit.Value.point;
                transform.rotation = Quaternion
                    .FromToRotation(Vector3.up, e.NewRaycastHit.Value.normal);
            }
        }

        private void Cursor_ItemReferenceChanged(object sender, ItemReferenceChangedEventArgs e)
        {
            if (e.NewReference != null)
            {
                _meshFilter.sharedMesh = e.NewReference.Item.Mesh;
            }
            else
                _meshFilter.sharedMesh = null;
        }
    }
}