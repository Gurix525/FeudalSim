using AI;
using Controls;
using Items;
using Misc;
using UnityEngine;
using World;
using Cursor = Controls.PlayerCursor;
using PlayerControls;

namespace Nature
{
    [RequireComponent(typeof(OutlineHandler))]
    public class Tree : MonoBehaviour, IMouseHandler
    {
        #region Public

        public void OnLeftMouseButton()
        {
            if (this == null)
                return;
            Item item = Item.Create("Wood", 20);
            Equipment.Insert(item);
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
            Player.Instance.Stats.AddSkill("Woodcutting", 1F);
        }

        #endregion Public
    }
}