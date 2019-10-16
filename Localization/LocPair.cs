using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlizzLoc
{
    [System.Serializable]
    public struct LocPair
    {
        public string DictKey;
        public string StringKey;

        public string GetString()
        {
            return Localization.GetString(DictKey, StringKey);
        }


    }
}
