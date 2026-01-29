using UnityEngine;
using Game.Runtime.Core;

public class InventorySlotsUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventorySlotUI[] slots;

    private void Awake()
    {
        if (slots == null || slots.Length == 0)
            slots = GetComponentsInChildren<InventorySlotUI>(true);
    }

    private void OnEnable()
    {
        Inventory.InventoryChangedEvent += OnInventoryChanged;
        Refresh();
    }

    private void OnDisable()
    {
        Inventory.InventoryChangedEvent -= OnInventoryChanged;
    }

    private void OnInventoryChanged()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (inventory == null || slots == null) return;

        for (int i = 0; i < slots.Length; i++)
            if (slots[i] != null) slots[i].Clear();

        var list = inventory.Entries;
        int count = Mathf.Min(list.Count, slots.Length);

        for (int i = 0; i < count; i++)
            if (slots[i] != null) slots[i].SetIcon(list[i].icon);
    }
}
