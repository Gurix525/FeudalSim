using AI;
using Controls;
using Extensions;
using Misc;
using UnityEngine;
using World;
using Cursor = Controls.MainCursor;
using Task = System.Threading.Tasks.Task;

namespace Items
{
    [RequireComponent(typeof(OutlineHandler))]
    public class ItemHandler : MonoBehaviour, IMouseHandler
    {
        #region Fields

        private bool _isClicked = false;
        private bool _isScattering = false;

        #endregion Fields

        #region Properties

        public Container Container { get; } = new(1);

        public Item Item => Container[0];

        #endregion Properties

        #region Public

        public async Task ScatterItem()
        {
            if (_isScattering)
                return;
            _isScattering = true;
            var meshCollider = GetComponent<MeshCollider>();
            meshCollider.convex = true;
            var rigidbody = gameObject.AddComponent<Rigidbody>();
            rigidbody.mass = (meshCollider.bounds.size.GetVolume() * 5F)
                .Clamp(1F, float.PositiveInfinity);
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            while (!rigidbody.IsSleeping())
            {
                await Task.Yield();
            }
            rigidbody.isKinematic = true;
            Destroy(rigidbody);
            meshCollider.convex = false;
            TerrainRenderer.MarkNavMeshToReload();
            _isScattering = false;
        }

        public void OnShiftLeftMouseButton(Vector2 position)
        {
            _isClicked = true;
        }

        public void OnMousePosition(Vector2 position)
        {
            if (Item == null)
                return;
            _isClicked = false;
            Cursor.Current.ItemReference = new(Container, 0);
        }

        public void OnShiftLeftMouseButtonRelase()
        {
            if (!_isClicked)
                return;
            _isClicked = false;
            Cursor.Current.RelaseItemReference();
            Item item = Container.ExtractAt(0);
            InventoryCanvas.InventoryContainer.Insert(item);
        }

        public void OnHoverEnd()
        {
            _isClicked = false;
        }

        #endregion Public

        #region Unity

        private void OnEnable()
        {
            Container.CollectionUpdated.AddListener(OnCollectionUpdated);
        }

        private void FixedUpdate()
        {
            if (transform.position.y < -10F)
            {
                transform.position += Vector3.up * 25F;
                GetComponent<Rigidbody>().velocity = Vector3.down * 0.2F;
            }
        }

        private void OnDisable()
        {
            Container.CollectionUpdated.RemoveListener(OnCollectionUpdated);
        }

        #endregion Unity

        #region Private

        private void OnCollectionUpdated()
        {
            if (Container[0] == null)
            {
                Destroy(gameObject);
                TerrainRenderer.MarkNavMeshToReload();
            }
        }

        #endregion Private
    }
}