using UnityEngine;

namespace Game.Runtime.Gameplay
{   
    public enum InvType { Item, Fragment }

    [System.Serializable]
    public class InvEntry
    {
        public InvType type;
        public string key;
        public Sprite icon;
    }
}
