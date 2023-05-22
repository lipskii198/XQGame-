using UnityEngine;

namespace _game.Scripts.Utility
{
    /// <summary>
    /// Simple non-lazy singleton implementation for MonoBehaviours that ensures that there is only one instance of the class
    /// </summary>
    public class LazySingletonMono<T> : MonoBehaviour where T : MonoBehaviour // Not really the lazy implementation heh
    {
        // Private instance - dont worry about it
        private static T _instance;

    
        // Public instance - we use this to access the singleton
        public static T Instance
        {
            get
            {
                // If the instance is already set, return it
                if (_instance != null) return _instance;
                // If the instance is not set, find it
                _instance = FindObjectOfType<T>();
                // If the instance is still not set, create it
                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                }
                // Return the instance
                return _instance;
            }
        }

        // Override the awake method to make sure the instance is set - Possibly problematic?
        protected virtual void Awake()
        {
            // Check if the instance of the class is null
            if (_instance == null)
            {
                // If it is, set it to this instance
                _instance = this as T;
            
                // Dont destroy this object on load
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                // If the instance of the class is not null, destroy this instance of the class
                Destroy(gameObject);
            }
        }
    }
}