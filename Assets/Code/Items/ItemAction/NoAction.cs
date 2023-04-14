using UnityEngine;

namespace Items
{
    public class NoAction : ItemAction
    {
        protected override Sprite GetSprite()
        {
            return Resources.Load<Sprite>("Sprites/Actions/NoAction");
        }
    }
}