using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlizzLoc
{
    [System.Serializable]
    public class LocalizationDict
    {
        public string key = "";
        public List<LocalizationString> strings = new List<LocalizationString>();

        public string GetString(string key, int lang_index)
        {
            foreach (var str in strings)
            {
                if (str.key == key)
                    return str.GetString(lang_index);
            }

            return "";
        }

    }

    [System.Serializable]
    public class LocalizationString
    {
        public string key = "";
        public List<string> strings = new List<string>();

        public void Repair()
        {
            var count = LocalizationHolder.Prefab.Languages.Count;
            while(strings.Count > count)
            {
                strings.RemoveAt(strings.Count - 1);
            }
            while(strings.Count < count)
            {
                strings.Add("");
            }
        }

        public string GetString()
        {
            return GetString(Localization.GetLanguage());
        }

        public string GetString(int language_index)
        {
            if (strings.Count <= language_index)
                Repair();

            return strings[language_index];
        }
    }
}