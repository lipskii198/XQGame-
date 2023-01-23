using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPoolManager : LazySingletonMono<ObjectPoolManager>
    {
        [SerializeField] private List<ObjectPoolItem> itemsToPool;
        [SerializeField] private List<GameObject> pooledObjects = new();
        [SerializeField] private Transform parent;

        private void Start()
        {
            BeginPooling();
        }

        public GameObject GetPooledObject(string objectTag)
        {
            // iterate through the list of pooled objects and check if the object is not active and has the same tag passed as a parameter
            foreach (var poolItem in pooledObjects.Where(poolItem => !poolItem.activeInHierarchy && poolItem.CompareTag(objectTag)))
            {
                // return the object if all is gucci
                return poolItem;
            }

            // iterate through the list of pooled objects and check if the object is not active and has the same tag passed as a parameter and can grow
            foreach (var obj in from poolItem in itemsToPool where poolItem.objectToPool.CompareTag(objectTag) && poolItem.canGrow select Instantiate(poolItem.objectToPool, parent, true))
            {
                // set the object to inactive, add it to the pooled objects list, and return it
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }

            // If all fails return null
            return null;
        }

        public void ClearPooledObjects()
        {
            foreach (var poolItem in pooledObjects)
                Destroy(poolItem);

            pooledObjects.Clear();
        }

        private void BeginPooling()
        {
            foreach (var poolItem in itemsToPool)
            {
                for (var i = 0; i < poolItem.amountToPool; i++)
                {
                    var obj = Instantiate(poolItem.objectToPool, parent);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                }
            }
        }
    }
}