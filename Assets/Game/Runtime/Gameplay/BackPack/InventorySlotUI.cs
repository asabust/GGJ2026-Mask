using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon; // 子物体 Icon 的 Image

    public void Clear()
    {
        if (icon == null) return;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void SetIcon(Sprite sprite)
    {
        if (icon == null) return;
        icon.sprite = sprite;
        icon.enabled = sprite != null;
    }
}
