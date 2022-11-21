using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers
{
    public class ObjectPoolManager : MonoBehaviour
    {
        // This script will be accessed by other scripts later so its best to just make it a singleton now
        public static ObjectPoolManager Instance;
        [SerializeField] private List<GameObject> pooledObjects;
        [SerializeField] private GameObject targetPrefab; //The prefab that we will use as a base 
        [SerializeField] private GameObject parent; //Where all these objects will be parented
        [SerializeField] private int amount; //Amount of objects to pool

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

        // Get the first pooled object that isn't currently active and return it otherwise just return the first in the list
        public GameObject GetPooledObject()
        {
            foreach (var x in pooledObjects.Where(x => !x.activeInHierarchy))
            {
                return x;
            }

            return pooledObjects.FirstOrDefault();
        }

        public void ClearPooledObjects()
        {
            foreach (var pooledObject in pooledObjects)
            {
                Destroy(pooledObject);
            }
            pooledObjects.Clear();
        }

        public void UpdatePooledObjects(GameObject prefab)
        {
            ClearPooledObjects();
            targetPrefab = prefab;
            BeginPooling();
        }

        private void BeginPooling()
        {
            for (var i = 0; i < amount; i++)
            {
                var obj = Instantiate(targetPrefab, parent.transform);
                obj.SetActive(false);
                pooledObjects.Add(obj);
            }
        }
    }
}