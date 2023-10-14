using AI;
using Controls;
using Misc;
using UnityEngine;
using World;
using Cursor = Controls.PlayerCursor;
using Task = System.Threading.Tasks.Task;

namespace Items
{
    [RequireComponent(typeof(OutlineHandler))]
    public class ItemHandler : MonoBehaviour, IMouseHandler, IDetectable
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
            Equipment.InventoryContainer.Insert(item);
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