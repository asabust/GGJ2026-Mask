using UnityEngine;
using Game.Runtime.Core;

public class InventorySlotsUI : MonoBehaviour
{
    [SerializeField] private FragmentInventory inventory;
    [SerializeField] private InventorySlotUI[] slots; // 5个

    public void Refresh()
    {
        if (inventory == null || slots == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null) slots[i].Clear();
        }

        var list = inventory.CollectedSprites;
        int count = Mathf.Min(list.Count, slots.Length);

        for (int i = 0; i < count; i++)
        {
            if (slots[i] != null) slots[i].SetIcon(list[i]);
        }
    }
}
