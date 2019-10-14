using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SlizzLoc
{
    public class WindowLocalizationEdit : EditorWindow
    {
        [MenuItem("Window/Localization Slizz")]
        public static void Show()
        {
            EditorWindow.GetWindow<WindowLocalizationEdit>();
        }

        //Visual
        private Texture2D _findTexture;



        //Body
        private LocalizationHolder holder;
        private FoldoutGroup _foldOutGroup = new FoldoutGroup();
        private string _newGroupName = "";
        private string _newKey = "";
        private List<string> _hideDict = new List<string>();
        private int _tab = 0;

        void OnEnable()
        {
            holder = LocalizationHolder.Prefab;
            if(_findTexture == null)
            {
                _findTexture = Resources.Load<Texture2D>("find");
            }
        }

        private bool _langFoldout = false;
        private string _newLang = "";
        private string _findLine = "";
        private GUISkin _skin;
        private GUISkin skin
        {
            get
            {
                if(_skin == null)
                {
                    _skin = Resources.Load<GUISkin>("StringEditor");
                }
                return _skin;
            }
        }



        public void OnGUI()
        {
            if (holder == null)
            {
                CantInit();
                return;
            }

            EditorGUI.BeginChangeCheck();
            _tab = GUILayout.Toolbar(_tab, new string[] { "Main", "Options"});
            switch (_tab)
            {
                case 0:
                    GUILayout.Space(10);
                    AddNewGroup();
                    DrawFinder();
                    GUILayout.Space(10);
                    DrawLocalizationGroups();
                    break;
                case 1:
                    GUILayout.Space(10);
                    DrawLanguageSelecter();
                    LanguagePart();
                    break;
            }
            

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(LocalizationHolder.Prefab);
            }
        }

        private void DrawFinder()
        {
            GUILayout.BeginHorizontal("box", GUILayout.Height(36));
            var rect = EditorGUILayout.GetControlRect(GUILayout.Width(32));
            rect.width = 30;
            rect.height = 30;
            EditorGUI.DrawTextureAlpha(rect, _findTexture);


            string findLine = EditorGUILayout.TextField("Find:", _findLine);
            if(findLine != _findLine)
            {
                _findLine = findLine;
                _hideDict.Clear();

                foreach(var dict in holder.dicts)
                {
                    bool hasSomething = false;
                    foreach(var str in dict.strings)
                    {
                        if (EqualToFindLine(str))
                        {
                            hasSomething = true;
                            break;
                        }
                    }
                    if (!hasSomething)
                    {
                        _hideDict.Add(dict.key);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        bool EqualToFindLine(LocalizationString locStr)
        {
            string findLine = _findLine.ToLower();
            if (locStr.key.ToLower().Contains(findLine))
                return true;

            foreach(var str in locStr.strings)
            {
                if (str.ToLower().Contains(findLine))
                    return true;
            }

            return false;
        }

        bool IsHidden(LocalizationDict dict)
        {
            if (string.IsNullOrEmpty(_findLine))
                return false;
            return _hideDict.Contains(dict.key);
        }


        private void AddNewGroup()
        {
            GUILayout.Space(10);
            GUILayout.BeginHorizontal("box");

            _newGroupName = EditorGUILayout.TextField("New group:", _newGroupName);
            if (GUILayout.Button("Add"))
            {
                holder.dicts.Add(new LocalizationDict() { key = _newGroupName
                });
                _newGroupName = "";
            }

            GUILayout.EndHorizontal();
        }

        private void AddNewKey(LocalizationDict dict)
        {
            GUILayout.BeginHorizontal("box");
            _newKey = EditorGUILayout.TextField("New key:", _newKey);
            if (GUILayout.Button("Add"))
            {
                dict.strings.Add(new LocalizationString()
                {
                    key = _newKey,
                    strings = new List<string>(holder.Languages.Count)
                });
                _newKey = "";
            }

            GUILayout.EndHorizontal();
        }

        private void DrawLocalizationGroups()
        {
            var defSkin = GUI.skin;
            int count = 0;
            foreach(var dict in holder.dicts)
            {
                foreach(var str in dict.strings)
                {
                    count++;
                }
            }
            int currentIndex = 0;
            for (int i = 0; i < holder.dicts.Count; i++)
            {
                var dict = holder.dicts[i];
                if (IsHidden(dict))
                    continue;

                GUILayout.BeginVertical("box");
                string key = dict.key + "_" + i;
                var value = _foldOutGroup.GUIFoldoutGroup(key, dict.key);
                if (value)
                {
                    if (GUILayout.Button("Remove"))
                    {
                        if (EditorUtility.DisplayDialog("Remove?", "Remove this group?", "Yes", "No"))
                        {
                            holder.dicts.RemoveAt(i);
                            return;
                        }
                    }
                    AddNewKey(dict);

                    foreach (var str in dict.strings)
                    {
                        if (!EqualToFindLine(str))
                            continue;
                        currentIndex += 1;
                        GUI.skin = skin;
                        GUILayout.BeginVertical("box");
                        bool _addFold = _foldOutGroup.GUIFoldoutGroup(key + "_" + currentIndex + "_" + str.key, str.key);
                        if (_addFold)
                        {
                            if (GUILayout.Button("Remove"))
                            {
                                if(EditorUtility.DisplayDialog("Remove?", "Remove this string?", "Yes", "No"))
                                {
                                    dict.strings.Remove(str);
                                    return;
                                }
                            }
                            str.key = EditorGUILayout.TextField("Key : ", str.key);
                            GUILayout.Space(10);
                            while (str.strings.Count < holder.Languages.Count)
                            {
                                str.strings.Add("");
                            }
                            while (str.strings.Count > holder.Languages.Count)
                            {
                                str.strings.RemoveAt(i);
                            }
                            for (int j = 0; j < holder.Languages.Count; j++)
                            {
                                str.strings[j] = EditorGUILayout.TextField(holder.Languages[j] + ":", str.strings[j]);
                            }
                        }
                        GUILayout.EndVertical();
                        GUI.skin = defSkin;
                    }
                }
                currentIndex += 1;
                GUILayout.EndVertical();
                GUILayout.Space(10);
            }
        }

        private void CantInit()
        {
            GUILayout.Label("Can't init", new GUIStyle() { fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter });
            if (GUILayout.Button("Init"))
            {
                var _holder = ScriptableObject.CreateInstance<LocalizationHolder>();

                var path = LocalizationHolder.PathToPrefab.Split('/');

                var currentPath = "";
                if(path.Length > 1)
                {
                    for(int i = 0; i < path.Length - 1; i++)
                    {
                        currentPath += path[i];
                        if (!System.IO.Directory.Exists("Assets/" + currentPath))
                        {
                            System.IO.Directory.CreateDirectory("Assets/" + currentPath);
                        }
                        currentPath += "/";
                    }
                }

                AssetDatabase.CreateAsset(_holder, "Assets/" + currentPath + path[path.Length - 1] + ".asset");
                AssetDatabase.SaveAssets();
                holder = LocalizationHolder.Prefab;
            }
            if (GUILayout.Button("Try again"))
            {
                holder = LocalizationHolder.Prefab;
            }

        }

        private void LanguagePart()
        {
            GUILayout.BeginVertical("box");
            _langFoldout = EditorGUILayout.Foldout(_langFoldout, "Languages", true);
            if (_langFoldout)
            {
                GUILayout.BeginHorizontal();
                _newLang = EditorGUILayout.TextField("New language:", _newLang);
                if (GUILayout.Button("Add"))
                {
                    holder.Languages.Add(_newLang);
                    _newLang = "";
                    AddRow();
                    return;
                }
                GUILayout.EndHorizontal();

                for (int i = 0; i < holder.Languages.Count; i++)
                {
                    var lang = holder.Languages[i];
                    GUILayout.BeginHorizontal();
                    lang = GUILayout.TextField(lang);
                    if (GUILayout.Button("Remove"))
                    {
                        holder.Languages.Remove(lang);
                        RemoveRow(i);
                        return;
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }

        private void AddRow()
        {
            foreach(var dict in holder.dicts)
            {
                foreach(var str in dict.strings)
                {
                    str.strings.Add("");
                }
            }
        }

        private void RemoveRow(int index)
        {
            foreach (var dict in holder.dicts)
            {
                foreach (var str in dict.strings)
                {
                    str.strings.RemoveAt(index);
                }
            }
        }


        private void DrawLanguageSelecter()
        {
            if(holder.Languages.Count == 0)
            {
                GUILayout.Label("Can't set current language, languages count = 0", new GUIStyle() { fontStyle = FontStyle.Bold });
            }
            else
            {
                string[] languages = holder.Languages.ToArray();

                int selectedIndex = EditorGUILayout.Popup("Current language :", holder.SelectedLangueage, languages);
                if(selectedIndex != holder.SelectedLangueage)
                {
                    holder.SetLanguage(selectedIndex);
                }
            }
        }

    }
}


