using UnityEngine;
using UnityEngine.UI;

public class QuickMenuSlot : MonoBehaviour
{
    [SerializeField] private Image _itemImage;

    private Image _background;

    public Image ItemImage => _itemImage;
    public Image Background => _background ??= GetComponent<Image>();
}