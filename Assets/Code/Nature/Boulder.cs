using Assets;
using Controls;
using Items;
using Misc;
using PlayerControls;
using UnityEngine;
using World;

namespace Nature
{
    [RequireComponent(typeof(OutlineHandler))]
    public class Boulder : MonoBehaviour, IMouseHandler
    {
        private float _destroyTime = 1F;
        private float _clickTime = 0F;
        private bool _destroying = false;

        #region Public

        public void OnLeftMouseButton(Vector2 position)
        {
            if (this == null)
                return;
            StartDestroying();
        }

        public void OnHoverEnd()
        {
            StopDestroying();
        }

        public void OnLeftMouseButtonRelase()
        {
            StopDestroying();
        }

        #endregion Public

        #region Unity

        private void FixedUpdate()
        {
            if (_destroying)
                _clickTime += Time.fixedDeltaTime;
            if (_clickTime > _destroyTime)
                DestroyBoulder();
        }

        #endregion Unity

        #region Private

        private void StartDestroying()
        {
            _clickTime = 0F;
            _destroying = true;
        }

        private void StopDestroying()
        {
            _destroying = false;
        }

        private void DestroyBoulder()
        {
            Item item = Item.Create("Stone", 5);
            InventoryCanvas.Insert(item);
            if (item.Count > 0)
                item.Drop(Player.Position);
            Destroy(gameObject);
            Player.Current.Stats.AddSkill("Digging", 1F);
            TerrainRenderer.MarkNavMeshToReload();
            AnalyticsBase.Add("natureDestroyed", "boulder");
        }

        #endregion Private
    }
}