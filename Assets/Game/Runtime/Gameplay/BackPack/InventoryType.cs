using UnityEngine;

namespace Game.Runtime.Gameplay
{
    public enum InvType { Item, Fragment }

    [System.Serializable]
    public class InvEntry
    {
        public InvType type;
        public string key;

        public string description;  // 观察文字

        public Sprite icon;         // 背包格子icon
    }
}
