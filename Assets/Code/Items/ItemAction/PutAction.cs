using UnityEngine;

namespace Items
{
    public class PutAction : ItemAction
    {
        protected override Sprite GetSprite()
        {
            return Sprites.GetSprite("PutAction");
        }
    }
}