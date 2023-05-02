using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utils {

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindOrCreate();
                }
                return _instance;
            }
        }

        private static T _instance;
        private static T FindOrCreate()
        {
            var instance = GameObject.FindObjectOfType<T>();

            if (instance != null)
            {
                return instance;
            }

            var name = typeof(T).Name + " Singleton";
            var containerGameObject = new GameObject(name);
            var singletonComponent = containerGameObject.AddComponent<T>();

            return singletonComponent;
        }
    }
}