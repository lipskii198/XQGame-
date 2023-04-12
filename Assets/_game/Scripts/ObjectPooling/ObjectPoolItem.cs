using System;
using UnityEngine;

namespace _game.Scripts.ObjectPooling
{
    [Serializable]
    public class ObjectPoolItem
    {
        public int amountToPool;
        public GameObject objectToPool;
        public bool canGrow;
    }
}