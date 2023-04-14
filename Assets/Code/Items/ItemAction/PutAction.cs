using UnityEngine;

namespace Items
{
    public class PutAction : ItemAction
    {
        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/PutAction");
        }
    }
}