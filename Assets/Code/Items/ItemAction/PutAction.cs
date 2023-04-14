using Controls;
using UnityEngine;
using Cursor = Controls.Cursor;

namespace Items
{
    public class PutAction : ItemAction
    {
        private float _meshRotation;

        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/PutAction");
        }

        public override void Update()
        {
            CursorItemMeshHighlight.SetMesh(Cursor.Item.Mesh);
            CursorMeshHighlight.SetMeshRotation(_meshRotation);
        }
    }
}