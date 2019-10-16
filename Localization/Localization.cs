using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

namespace SlizzLoc
{
    public static class Localization
    {

        private static bool _DebugModeEnable = false;

        public static string GetString(string groupKey, string key)
        {
            if (ShowErrorNonPrefab())
                return "";

            var dictBody = LocalizationHolder.Prefab.dicts.Find(d => d.key == groupKey);
            if(dictBody == null)
            {
                if (_DebugModeEnable)
                    Debug.LogError("Can't find group with key " + groupKey);
                return "";
            }
            var str = dictBody.strings.Find(s => s.key == key);
            if(str == null)
            {
                if (_DebugModeEnable)
                    Debug.LogError("Can't find string with key " + key);
                return "";
            }
            return str.GetString();
        }

        public static string GetString(int groupIndex, string key)
        {
            if (ShowErrorNonPrefab())
                return "";

            if(groupIndex >= LocalizationHolder.Prefab.dicts.Count)
            {
                if (_DebugModeEnable)
                    Debug.LogError("Can't find group with index " + groupIndex);
                return "";
            }
            return GetString(LocalizationHolder.Prefab.dicts[groupIndex].key, key);
        }

        public static LocalizationString GetStringAsObject(string groupKey, string key)
        {
            if (ShowErrorNonPrefab())
                return null;

            var dictBody = LocalizationHolder.Prefab.dicts.Find(d => d.key == groupKey);
            if (dictBody == null)
            {
                if (_DebugModeEnable)
                    Debug.LogError("Can't find group with key " + groupKey);
                return null;
            }
            var str = dictBody.strings.Find(s => s.key == key);
            if (str == null)
            {
                if (_DebugModeEnable)
                    Debug.LogError("Can't find string with key " + key);
                return null;
            }
            return str;
        }

        public static void AddString(string groupKey, string key, params string[] strings)
        {
            if (ShowErrorNonPrefab())
                return;

            var dictBody = LocalizationHolder.Prefab.dicts.Find(d => d.key == groupKey);
            if (dictBody == null)
            {
                if (_DebugModeEnable)
                    Debug.LogError("Can't find group with key " + groupKey);
                return;
            }

            dictBody.strings.Add(new LocalizationString() { key = key, strings = new List<string>(strings) });
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(LocalizationHolder.Prefab);
#endif
        }

        public static void AddGroup(string key)
        {
            if (ShowErrorNonPrefab())
                return;

            LocalizationHolder.Prefab.dicts.Add(new LocalizationDict() { key = key });
        }
        
        public static LocalizationDict GetGroup(string key)
        {
            if (ShowErrorNonPrefab())
                return null;

            var dictBody = LocalizationHolder.Prefab.dicts.Find(d => d.key == key);
            if (dictBody == null)
            {
                if (_DebugModeEnable)
                    Debug.LogError("Can't find group with key " + key);
                return null;
            }

            return dictBody;
        }

        public static int GetLanguage()
        {
            if (LocalizationHolder.Prefab == null)
            {
                ShowErrorNonPrefab();
                return -1;
            }
            return LocalizationHolder.Prefab.SelectedLangueage;
        }

        public static void SetLanguage(int index)
        {
            if(LocalizationHolder.Prefab == null)
            {
                ShowErrorNonPrefab();
                return;
            }
        }

        public static int AddLanguage(string lang)
        {
            if (LocalizationHolder.Prefab == null)
            {
                ShowErrorNonPrefab();
                return -1;
            }
            LocalizationHolder.Prefab.Languages.Add(lang);
            return LocalizationHolder.Prefab.Languages.Count - 1;
        }

        public static void RemoveLanguage(int index)
        {
            if (ShowErrorNonPrefab())
                return;
            var pref = LocalizationHolder.Prefab;
            pref.Languages.RemoveAt(index);
            foreach(var dict in pref.dicts)
            {
                dict.strings.RemoveAt(index);
            }
        }



        static bool ShowErrorNonPrefab()
        {
            if(LocalizationHolder.Prefab == null)
            {
                if (_DebugModeEnable)
                    Debug.LogError("There are no LocalizationHolder prefab. Please open Window/Localization Slizz and init!");
                return true;
            }
            return false;
        }
    }
}
