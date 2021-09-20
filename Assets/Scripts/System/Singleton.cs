using System;
using UnityEngine;

namespace Scripts
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Object reference
        /// </summary>
        public static T S { get; private set; }

        private void Awake()
        {
            if (S != null)
            {
                throw new SingletonException();
            }

            S = GetComponent<T>();
            
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
    }
}