using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SlizzLoc
{
    [CustomEditor(typeof(Localization))]
    public class LocalizationEditor : Editor
    {

        static bool show_languages = false;
        static bool[] show_dict;
        static List<string> showed = new List<string>();
        Texture2D grey_text;
        static string New_list_name = "Default name";
        static string New_string_name = "default";
        static string New_language_name = "default";
        Dictionary<Color, Texture2D> textures = new Dictionary<Color, Texture2D>();

        private void OnEnable()
        {
            OnDictsUpdate();
            showed.Clear();
            grey_text = GetGreyTexture();
            Localization main = (Localization)target;
            main.OnLoad = OnDictsUpdate;
        }

        public void OnDictsUpdate()
        {
            Localization main = (Localization)target;
            show_dict = new bool[main.GetDictsCount()];
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            Localization main = (Localization)target;
            show_languages = GUILayout.Toggle(show_languages, "Languages list");
            if (show_languages)
            {
                StartLevel(20);
                ShowLanguages(main);
                EndLevel();
            }

            GUILayout.Space(20);
            GUILayout.Label("Lists:");
            for (int i = 0; i < main.dicts.Count; i++)
            {

                show_dict[i] = GUILayout.Toggle(show_dict[i], main.dicts[i].key);
                var top_r = GUILayoutUtility.GetLastRect();

                var bot_r = GUILayoutUtility.GetLastRect();
                if (show_dict[i])
                {
                    ShowDict(main, main.dicts[i]);
                    bot_r = GUILayoutUtility.GetLastRect();
                }
                var res = new Rect(top_r.x, top_r.y, top_r.width, bot_r.yMax - top_r.yMin);
                DrawCustomLayer(res, 68, 68, 68, .2f);
            }
            GUILayout.Space(20);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name:", GUILayout.Width(50));
            New_list_name = GUILayout.TextField(New_list_name);
            if (GUILayout.Button("Add new list", GUILayout.Width(100)))
            {
                Undo.RecordObject(target, "New list at localization");
                main.dicts.Add(new Localization_dict() { key = New_list_name });
                OnDictsUpdate();
            }
            GUILayout.EndHorizontal();
            if (GUILayout.Button("Save to file"))
            {
                main.SaveToFile();
            }
            if (GUILayout.Button("Load from file"))
            {
                main.LoadFromFile();
            }
        }

        void ShowLanguages(Localization main)
        {
            for (int i = 0; i < main.languages.Count; i++)
            {
                GUILayout.BeginHorizontal();
                string cached = main.languages[i];
                cached = GUILayout.TextField(cached);
                main.languages[i] = cached;
                if (i != main.Current_language_index)
                {
                    if (GUILayout.Button("Set", GUILayout.Width(100)))
                    {
                        Undo.RecordObject(target, "Set language at localization");
                        main.ChangeLanguage(i);
                    }
                    if (GUILayout.Button("[X]Remove", GUILayout.Width(100)))
                    {
                        Undo.RecordObject(main, "Remove language at localization");
                        main.RemoveLanguageAtIndex(i);
                        main.ChangeLanguage(0);
                        return;
                    }
                }
                else
                {
                    if (GUILayout.Button("[X]Remove", GUILayout.Width(100)))
                    {
                        Undo.RecordObject(main, "Remove language at localization");
                        main.RemoveLanguageAtIndex(i);
                        return;
                    }
                }
                DrawCustomLayer(18, 250, 0, 0);

                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Name:", GUILayout.Width(50));
            New_language_name = GUILayout.TextField(New_language_name);
            if (GUILayout.Button("Add language"))
            {
                Undo.RecordObject(target, "New language at localization");
                main.languages.Add(New_language_name);
            }
            GUILayout.EndHorizontal();
        }

        void StartLevel(int offset)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginVertical();
        }

        void EndLevel()
        {
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        void ShowDict(Localization main, Localization_dict dict)
        {
            StartLevel(20);
            dict.key = GUILayout.TextField(dict.key);

            GUILayout.Label("Keys:");
            var s_r = GUILayoutUtility.GetLastRect();
            foreach (var str in dict.strings)
            {

                StartLevel(20);

                bool cached = showed.Contains(str.key);
                bool old = cached;

                cached = GUILayout.Toggle(cached, str.key);
                if (cached != old)
                {
                    if (cached && !showed.Contains(str.key))
                    {
                        showed.Add(str.key);
                    }
                    else if (!cached && showed.Contains(str.key))
                    {
                        showed.Remove(str.key);
                    }
                }

                if (cached)
                {
                    StartLevel(20);
                    string old_name = str.key;
                    str.key = GUILayout.TextField(str.key);
                    if (old_name != str.key)
                    {
                        showed.Remove(old_name);
                        showed.Add(str.key);
                    }
                    for (int i = 0; i < main.languages.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(main.languages[i], GUILayout.Width(80));
                        str.strings[i] = GUILayout.TextField(str.GetString(i));
                        GUILayout.EndHorizontal();
                    }
                    if (GUILayout.Button("remove"))
                    {
                        Undo.RecordObject(target, "Remove string at localization");
                        dict.strings.Remove(str);
                        return;
                    }
                    EndLevel();
                }
                EndLevel();
            }
            var e_r = GUILayoutUtility.GetLastRect();
            var res = new Rect(s_r.x, s_r.y, s_r.width, e_r.yMax - s_r.yMin);
            DrawCustomLayer(res, 68, 68, 68, .1f);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Name:", GUILayout.Width(50));
            New_string_name = GUILayout.TextField(New_string_name);
            if (GUILayout.Button("Add new key"))
            {
                Undo.RecordObject(target, "New string at localization");
                dict.strings.Add(new Localization_string() { key = New_string_name });
            }
            GUILayout.EndHorizontal();

            if (GUILayout.Button("[X]Remove list"))
            {
                Undo.RecordObject(target, "Remove list at localization");
                main.dicts.Remove(dict);
                OnDictsUpdate();
            }
            EndLevel();
        }

        Texture2D GetGreyTexture()
        {
            Texture2D text = new Texture2D(1, 1);
            Color col = new Color(100f / 255f, 100f / 255f, 100f / 255f, 0.3f);
            col.a = 0.3f;
            text.SetPixel(0, 0, col);
            text.Apply();
            return text;
        }

        Texture2D GetTextureByColor(float r, float g, float b, float a)
        {
            Color col = new Color(r / 255f, g / 255f, b / 255f, a);
            if (textures.ContainsKey(col))
            {
                return textures[col];
            }
            Texture2D text = new Texture2D(1, 1);
            text.SetPixel(0, 0, col);
            text.Apply();
            textures.Add(col, text);
            return text;
        }

        Texture2D GetRedTexture()
        {
            Texture2D text = new Texture2D(1, 1);
            Color col = new Color(255f / 255f, 0, 0, 0.3f);
            col.a = 0.3f;
            text.SetPixel(0, 0, col);
            text.Apply();
            return text;
        }



        void DrawCustomLayer(float height, float r, float g, float b, float a = 0.3f)
        {
            Rect rect = GUILayoutUtility.GetLastRect();
            GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, height), GetTextureByColor(r, g, b, a));
        }

        void DrawCustomLayer(Rect rect, float r, float g, float b, float a = 0.3f)
        {
            GUI.DrawTexture(rect, GetTextureByColor(r, g, b, a));
        }
    }

}
