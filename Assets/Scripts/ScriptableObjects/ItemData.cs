using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 0)]
    public class ItemData : ScriptableObject
    {
        [SerializeField] private int itemId;
        [SerializeField] private string itemName;
        [SerializeField] private int maxStack;
        
        public int ItemId => itemId;
        public string ItemName => itemName;
        public int MaxStack => maxStack;
    }
}