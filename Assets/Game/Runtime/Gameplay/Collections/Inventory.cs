using System;
using System.Collections.Generic;
using UnityEngine;
using Game.Runtime.Data;
using Game.Runtime.Gameplay;

namespace Game.Runtime.Core
{
    public class Inventory : MonoBehaviour
    {
        [Header("Config: Drag all AnomalySO here")]
        [SerializeField] private List<AnomalySO> anomalyList = new List<AnomalySO>();
        [SerializeField] private List<ItemSO> itemList = new List<ItemSO>();

        private Dictionary<string, AnomalySO> fragmentMap;
        private Dictionary<string, ItemSO> itemMap;

        private readonly HashSet<string> collectedFragmentKeys = new HashSet<string>();
        private readonly HashSet<string> collectedItemKeys = new HashSet<string>();
        private readonly List<InvEntry> entries = new List<InvEntry>();

        public IReadOnlyList<InvEntry> Entries => entries;

        public static event Action InventoryChangedEvent;

        private void Awake()
        {
            BuildFragmentMap();
            BuildItemMap();
        }

        private void OnEnable()
        {
            EventHandler.FragmentCollectedEvent += OnFragmentCollected;
            EventHandler.ItemCollectedEvent += OnItemCollected;
        }

        private void OnDisable()
        {
            EventHandler.FragmentCollectedEvent -= OnFragmentCollected;
            EventHandler.ItemCollectedEvent -= OnItemCollected;
        }

        private void BuildFragmentMap()
        {
            fragmentMap = new Dictionary<string, AnomalySO>();
            for (int i = 0; i < anomalyList.Count; i++)
            {
                var so = anomalyList[i];
                if (so == null) continue;
                if (so.fragmentName == FragmentName.None) continue;

                string key = so.fragmentName.ToString();
                if (!fragmentMap.ContainsKey(key)) fragmentMap.Add(key, so);
            }
        }

        private void BuildItemMap()
        {
            itemMap = new Dictionary<string, ItemSO>();
            for (int i = 0; i < itemList.Count; i++)
            {
                var so = itemList[i];
                if (so == null) continue;

                string key = so.itemName;
                if (string.IsNullOrWhiteSpace(key)) continue;
                if (!itemMap.ContainsKey(key)) itemMap.Add(key, so);
            }
        }

        private void OnFragmentCollected(string fragmentName)
        {
            if (string.IsNullOrWhiteSpace(fragmentName)) return;
            if (fragmentMap == null) BuildFragmentMap();

            if (!fragmentMap.TryGetValue(fragmentName, out var so) || so == null) return;
            if (!collectedFragmentKeys.Add(fragmentName)) return; // 碎片去重

            entries.Add(new InvEntry {
                type = InvType.Fragment,
                key = fragmentName,
                icon = so.fragmentSprite,
                description = so.fragmentDescription
            });

            InventoryChangedEvent?.Invoke();
        }

        private void OnItemCollected(string itemName)
        {
            if (string.IsNullOrWhiteSpace(itemName)) return;
            if (itemMap == null) BuildItemMap();

            if (!itemMap.TryGetValue(itemName, out var so) || so == null) return;
            if (!collectedItemKeys.Add(itemName)) return; // 物品去重
            if (so.icon == null) return;

            entries.Add(new InvEntry {
                type = InvType.Item,
                key = itemName,
                description = so.description,
                icon = so.icon
            });

            InventoryChangedEvent?.Invoke();
        }

        public void ResetInventory()
        {
            collectedFragmentKeys.Clear();
            collectedItemKeys.Clear();
            entries.Clear();
            InventoryChangedEvent?.Invoke();
        }
    }
}
