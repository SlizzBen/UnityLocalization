using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homebrew
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        private static T _instance;
        public static System.Object _lock = new System.Object();
        public static T Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = FindObjectOfType<T>();
                        if (_instance == null)
                        {
                            Debug.LogError("There are no " + typeof(T) + " on scene!");
                            return null;
                        }
                    }
                    return _instance;
                }
            }
        }
    }
}
