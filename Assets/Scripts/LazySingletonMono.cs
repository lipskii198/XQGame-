using System;
using UnityEngine;

public class LazySingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
        // Base generic class for a lazy singleton - the current issues with this is that LazyInstance needs to be linked to the object itself
        // otherwise it will create a new one and we will basically be using an invalid instance
        // But im going with the Lazy<T> pattern due to it being thread safe and then we basically just pass the value of it to Instance
        //TODO: implement lazy singleton pattern with Unity
        private static readonly Lazy<T> LazyInstance = new();
        public static T Instance => LazyInstance.Value;
}