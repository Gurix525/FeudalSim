using System.Linq;
using UnityEngine;

public class Sprites : MonoBehaviour
{
    [SerializeField] private Sprite[] _sprites;

    private static Sprites _instance;

    private void Awake()
    {
        _instance = this;
    }

    public static Sprite GetSprite(string name)
    {
        return _instance._sprites.ToList().Find(sprite => sprite.name == name);
    }
}