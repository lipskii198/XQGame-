using System;
using UnityEngine;

namespace ObjectPooling
{
    [Serializable]
    public class ObjectPoolItem
    {
        public int amountToPool;
        public GameObject objectToPool;
        public bool canGrow;
    }
}