using UnityEngine;

[DisallowMultipleComponent]
public class Hearing : Sense
{
    public override bool IsObjectPerceptible(GameObject gameObject)
    {
        if (!IsObjectInPerceptingRange(gameObject))
            return false;
        return false;
    }
}