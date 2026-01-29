using UnityEngine;
using Game.Runtime.Core;

public class NormalItemInteractable : Interactable
{
    [Header("Item Config")]
    [SerializeField] private string itemName;   // must match ItemSO.itemName in Inventory.itemList
    [SerializeField] private bool destroyOnPickup = true;

    private bool picked;

    public override void Interact()
    {
        if (picked) return;
        if (string.IsNullOrWhiteSpace(itemName)) return;

        picked = true;
        Debug.Log($"[Item] CallItemCollectedEvent({itemName})");

        EventHandler.CallItemCollectedEvent(itemName);

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
        else
        {
            // 防止二次交互
            var col2d = GetComponent<Collider2D>();
            if (col2d != null) col2d.enabled = false;
        }
    }
}
