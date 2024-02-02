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
    public class Tree : MonoBehaviour, IMouseHandler
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
                DestroyTree();
        }

        #endregion

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

        private void DestroyTree()
        {
            Item item = Item.Create("Wood", 20);
            InventoryCanvas.Insert(item);
            if (item.Count > 0)
                item.Drop(Player.Position);
            //GameObject log = Instantiate(
            //    Resources.Load<GameObject>("Prefabs/Nature/Log"),
            //    transform.parent.parent.parent);
            //log.transform.position = transform.position;
            //log.transform.localScale = transform.parent.localScale;
            //log.GetComponent<TemporaryRigidbody>().ActivateRigidbody(
            //    Vector3.forward * 100F,
            //    log.transform.position + Vector3.up * 10F,
            //    200F);
            Destroy(transform.parent.gameObject);
            TerrainRenderer.MarkNavMeshToReload();
            Player.Current.Stats.AddSkill("Woodcutting", 1F);
            AnalyticsBase.Add("natureDestroyed", "boulder");
        }

        #endregion Private
    }
}