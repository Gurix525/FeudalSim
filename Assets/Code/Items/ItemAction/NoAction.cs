using UnityEngine;

namespace Items
{
    public class NoAction : ItemAction
    {
        protected override Sprite GetSprite()
        {
            return Sprites.GetSprite("NoAction");
        }
    }
}