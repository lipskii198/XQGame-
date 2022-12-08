using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ObjectPooling
{
    public class ObjectPoolManager : MonoBehaviour
    {
        // This script will be accessed by other scripts later so its best to just make it a singleton now
        public static ObjectPoolManager Instance;
        [SerializeField] private List<ObjectPoolItem> itemsToPool; // List of all the items we want to pool
        [SerializeField] private List<GameObject> pooledObjects; // List of all the pooled objects
        [SerializeField] private GameObject parent; //Where all these objects will be parented

        private void Awake()
        {
            //This is fine for now to create singletons but moving forward we would probably want a generic class to automate all this - Look at LazySingletonMono
            if (Instance != null && Instance != this)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // we probably want this to work cross-scenes so lets not destroy it
            }
        }

        private void Start()
        {
            pooledObjects = new List<GameObject>();
            BeginPooling();
        }

        // Get the first pooled object with a tag and return it - if none are available, return null but if the pooled item is allowed to grow then create a new one
        public GameObject GetPooledObject(string tag)
        {
            foreach (var t in pooledObjects.Where(t => !t.activeInHierarchy && t.CompareTag(tag))) // Get all the inactive objects with the tag
            {
                return t;
            }

            foreach (var obj in from poolItem in itemsToPool where poolItem.objectToPool.CompareTag(tag) where poolItem.canGrow select Instantiate(poolItem.objectToPool)) // Get all the items that can grow and have the tag
            {
                obj.SetActive(false);
                pooledObjects.Add(obj);
                return obj;
            }
            
            return null;
        }

        public void ClearPooledObjects()
        {
            foreach (var pooledObject in pooledObjects)
            {
                Destroy(pooledObject);
            }
            pooledObjects.Clear();
        }

        [Obsolete("This method is deprecated, needs to be refactored to use the new ObjectPoolItem class")]
        public void UpdatePooledObjects(GameObject prefab)
        {
            
        }

        private void BeginPooling()
        {
            foreach (var poolItem in itemsToPool)
            {
                for (var i = 0; i < poolItem.amountToPool; i++)
                {
                    var obj = Instantiate(poolItem.objectToPool, parent.transform);
                    obj.SetActive(false);
                    pooledObjects.Add(obj);
                }
            }
        }
    }
}