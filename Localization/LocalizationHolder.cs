using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlizzLoc
{
   
    public class LocalizationHolder : ScriptableObject
    {
        static public string PathToPrefab = "Resources/LocalizationHolder";
        static private LocalizationHolder _prefab;
        static public LocalizationHolder Prefab
        {
            get
            {
                if(_prefab == null)
                {
                    _prefab = Resources.Load<LocalizationHolder>("LocalizationHolder");
                }
                return _prefab;
            }
        }

        public System.Action<int> OnChangeLanguage;

        public int SelectedLangueage = 0;
        public List<string> Languages = new List<string>();

        public List<LocalizationDict> dicts = new List<LocalizationDict>();

        public void SetLanguage(int index)
        {
            if (SelectedLangueage == index)
                return;
            SelectedLangueage = index;
            if(OnChangeLanguage != null)
            {
                OnChangeLanguage(index);
            }
        }

    }
}
