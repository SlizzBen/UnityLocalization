using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

namespace SlizzLoc
{
    public class Localization : Singleton<Localization>
    {


        public int Current_language_index;
        public List<string> languages = new List<string>();
        public List<Localization_dict> dicts = new List<Localization_dict>();
        public System.Action OnLoad;

        public System.Action OnChangeLanguage;

        /// <summary>
        /// Get string by key
        /// </summary>
        /// <param name="dict">Name of list</param>
        /// <param name="key">Key of string</param>
        /// <returns></returns>
        public static string GetString(string dict, string key)
        {

            foreach (var d in Instance.dicts)
            {
                if (d.key == dict)
                {
                    return d.GetString(key, Instance.Current_language_index);
                }
            }

            Debug.LogError("[Localization]There are no dict with name " + dict);
            return "";
        }

        public static bool HasString(string dict, string key)
        {

            foreach (var d in Instance.dicts)
            {
                if (d.key == dict)
                {
                    foreach(var str in d.strings)
                    {
                        if (str.key == key)
                            return true;
                    }
                }
            }

            return false;
        }

        public Localization_dict GetDictByKey(string key)
        {
            foreach(var dict in dicts)
            {
                if (dict.key == key)
                    return dict;
            }
            return null;
        }

        public void RemoveLanguageAtIndex(int ind)
        {
            languages.RemoveAt(ind);
            foreach (var d in dicts)
            {
                foreach (var str in d.strings)
                {
                    if(str.strings.Count > ind)
                    str.strings.RemoveAt(ind);
                }
            }
        }

        private void Start()
        {
            GetLanguage();
        }

        public void ChangeLanguage(int index)
        {
            Current_language_index = index;
            Save();
            if (OnChangeLanguage != null)
                OnChangeLanguage();

        }


        private string LangStringParam = "Language_param";

        void GetLanguage()
        {
            int i = PlayerPrefs.GetInt(LangStringParam);
            ChangeLanguage(i);
        }

        void Save()
        {
            int i = Current_language_index;
            PlayerPrefs.SetInt(LangStringParam, i);
        }

#if UNITY_EDITOR

        public void SaveToFile()
        {
            string save_file_path = "Assets/localization.json";
            Localization_save_data save_file = new Localization_save_data();
            save_file.lists = new List<Localization_dict>(dicts);
            save_file.languages = new List<string>(languages);
            if (!System.IO.File.Exists(save_file_path))
            {
                var writer = System.IO.File.Create(save_file_path);
                writer.Close();
            }
            System.IO.File.WriteAllText(save_file_path, JsonUtility.ToJson(save_file));
            Debug.Log("Saved to " + save_file_path);
        }

        public void LoadFromFile()
        {
            string save_file_path = "Assets/localization.json";
            if (System.IO.File.Exists(save_file_path) == false)
            {
                Debug.Log("No file");
                return;
            }
            string text = System.IO.File.ReadAllText(save_file_path);
            Localization_save_data save_file = JsonUtility.FromJson<Localization_save_data>(text);
            dicts = new List<Localization_dict>(save_file.lists);
            languages = new List<string>(save_file.languages);
            UnityEditor.EditorUtility.SetDirty(gameObject);
            if (OnLoad != null)
                OnLoad();
        }

#endif

        public int GetLanguagesCount()
        {
            return languages.Count;
        }

        public int GetDictsCount()
        {
            return dicts.Count;
        }
    }

    [System.Serializable]
    public class Localization_save_data
    {
        public List<Localization_dict> lists = new List<Localization_dict>();
        public List<string> languages = new List<string>();
    }


    [System.Serializable]
    public class Localization_dict
    {
        public string key = "";
        public List<Localization_string> strings = new List<Localization_string>();

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
    public class Localization_string
    {
        public string key = "";
        public List<string> strings = new List<string>();

        void Repair()
        {
            var count = Localization.Instance.GetLanguagesCount();
            for (int i = strings.Count; i < count; i++)
            {
                strings.Add("");
            }
        }

        public string GetString(int language_index)
        {
            if (strings.Count <= language_index)
                Repair();

            return strings[language_index];
        }
    }


    [System.Serializable]
    public class Localization_dictionary_list
    {
        public string Name;
        public List<Localization_dict> strings = new List<Localization_dict>();
    }


    public enum ELanguages
    {
        Rus,
        Eng
    }
}
