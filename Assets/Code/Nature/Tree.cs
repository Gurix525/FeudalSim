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
        #region Public

        public void OnLeftMouseButton(Vector2 position)
        {
            if (this == null)
                return;
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

        #endregion Public
    }
}