using System.Collections.Generic;
using UnityEngine;
using Game.Runtime.Data;

namespace Game.Runtime.Core
{
    public class FragmentInventory : MonoBehaviour
    {
        [Header("Config: Drag all AnomalySO here")]
        [SerializeField] private List<AnomalySO> anomalyList = new List<AnomalySO>();

        // fragmentName(string) -> AnomalySO（拿 sprite/description）
        private Dictionary<string, AnomalySO> fragmentMap;

        private readonly HashSet<string> collectedKeys = new HashSet<string>();
        private readonly List<Sprite> collectedSprites = new List<Sprite>();

        public IReadOnlyList<Sprite> CollectedSprites => collectedSprites;

        private void Awake()
        {
            BuildMap();
        }

        private void OnEnable()
        {
            EventHandler.FragmentCollectedEvent += OnFragmentCollected;
        }

        private void OnDisable()
        {
            EventHandler.FragmentCollectedEvent -= OnFragmentCollected;
        }

        private void BuildMap()
        {
            fragmentMap = new Dictionary<string, AnomalySO>();

            for (int i = 0; i < anomalyList.Count; i++)
            {
                var so = anomalyList[i];
                if (so == null) continue;
                if (so.fragmentName == FragmentName.None) continue;

                string key = so.fragmentName.ToString();
                if (!fragmentMap.ContainsKey(key))
                    fragmentMap.Add(key, so);
            }
        }

        private void OnFragmentCollected(string fragmentName)
        {
            if (string.IsNullOrWhiteSpace(fragmentName)) return;
            if (fragmentMap == null) BuildMap();

            if (!fragmentMap.TryGetValue(fragmentName, out var so) || so == null) return;

            // 去重：同一个碎片只收一次
            if (!collectedKeys.Add(fragmentName)) return;

            collectedSprites.Add(so.fragmentSprite);
        }

        public void ResetInventory()
        {
            collectedKeys.Clear();
            collectedSprites.Clear();
        }
    }
}
