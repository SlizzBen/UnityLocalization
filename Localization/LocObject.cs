using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlizzLoc
{
    public class LocObject : MonoBehaviour
    {

        public bool UpdateOnAwake = true;
        public string GroupKey = "";
        public string StringKey = "";

        private void Awake()
        {
            OnAwake();
            if (UpdateOnAwake)
                UpdateText();
        }

        protected virtual void OnAwake()
        {

        }

        public virtual void UpdateText()
        {

        }


    }
}
