using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace SlizzLoc
{
    [CustomPropertyDrawer(typeof(LocPair))]
    public class LocPairEditor : PropertyDrawer
    {

        Rect _position;
        float _lastY = 0;
        float _lastHeight;
        string _lastDictKey = "";
        LocalizationDict _dict;
        bool _init = false;
        string _value = "";
        Rect GetNextLine()
        {
            var rect = new Rect(x: _position.x,
                y: _position.y + _lastY,
                width: _position.width,
                height: EditorGUIUtility.singleLineHeight);
            _lastY += EditorGUIUtility.singleLineHeight;
            return rect;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if(LocalizationHolder.Prefab == null)
            {
                return EditorGUIUtility.singleLineHeight;
            }
            return _lastHeight;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            
            _lastY = 0;
            _position = position;
            var curRect = GetNextLine();
            if (LocalizationHolder.Prefab == null)
            {
                EditorGUI.LabelField(curRect, "Init in Windiw/Localization Slizz");
                return;
            }
            EditorGUI.LabelField(curRect, label, new GUIStyle() { fontStyle = FontStyle.Bold });

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 1;
            string dictKey = "";
            string stringKey = "";
            {
                var line = GetNextLine();
                dictKey = DrawSelectDictKey(line, property.FindPropertyRelative("DictKey").stringValue);
                property.FindPropertyRelative("DictKey").stringValue = dictKey;
                if (dictKey != _lastDictKey || !_init)
                {
                    _init = true;
                    _lastDictKey = dictKey;
                    _dict = LocalizationHolder.Prefab.dicts.Find(d => d.key == dictKey);
                }
            }
            if (_dict != null)
            {
                var line = GetNextLine();
                stringKey = DrawSelectStringKey(line, property.FindPropertyRelative("StringKey").stringValue);
                property.FindPropertyRelative("StringKey").stringValue = stringKey;
            }

            if(!string.IsNullOrEmpty(stringKey) && !string.IsNullOrEmpty(dictKey))
            {
                var obj = Localization.GetStringAsObject(dictKey, stringKey);
                if(obj != null)
                {
                    obj.Repair();
                    for (int i = 0; i < LocalizationHolder.Prefab.Languages.Count; i++)
                    {
                        var line = GetNextLine();
                        string value = EditorGUI.TextField(line, new GUIContent(LocalizationHolder.Prefab.Languages[i] + ":"), obj.strings[i]);
                        if (value != obj.strings[i])
                        {
                            obj.strings[i] = value;
                            LocalizationHolder.Save();
                        }
                    }
                }
                
                
            }


            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            var lastLine = GetNextLine();
            float height = lastLine.y + lastLine.height - position.y;
            if(_lastHeight < height)
            {
                _lastHeight = height;
            }
        }

        private string DrawSelectDictKey(Rect line, string stringValue)
        {
            var fLine = new Rect(
                line.x,
                line.y,
                EditorGUIUtility.currentViewWidth * 0.1f,
                line.height
                );
            EditorGUI.LabelField(fLine, "Dict Key:");

            var sLine = new Rect(
                line.x + EditorGUIUtility.currentViewWidth * 0.1f,
                line.y,
                EditorGUIUtility.currentViewWidth * 0.4f,
                line.height
                );

            string input = EditorGUI.TextField(sLine, stringValue);
            var tLine = new Rect(
                line.x + EditorGUIUtility.currentViewWidth * 0.5f,
                line.y,
                EditorGUIUtility.currentViewWidth * 0.4f,
                line.height
                );

            string popup = SelectDict(tLine, stringValue);

            if(input != stringValue)
            {
                return input;
            }
            return popup;
        }

        private string DrawSelectStringKey(Rect line, string stringValue)
        {
            var fLine = new Rect(
                line.x,
                line.y,
                EditorGUIUtility.currentViewWidth * 0.1f,
                line.height
                );
            EditorGUI.LabelField(fLine, "String Key:");

            var sLine = new Rect(
                line.x + EditorGUIUtility.currentViewWidth * 0.1f,
                line.y,
                EditorGUIUtility.currentViewWidth * 0.4f,
                line.height
                );

            string input = EditorGUI.TextField(sLine, stringValue);
            var tLine = new Rect(
                line.x + EditorGUIUtility.currentViewWidth * 0.5f,
                line.y,
                EditorGUIUtility.currentViewWidth * 0.4f,
                line.height
                );

            string popup = SelectString(tLine, stringValue);

            if (input != stringValue)
            {
                Debug.Log("return " + input);
                return input;
            }
            return popup;
        }


        string SelectDict(Rect r, string Value)
        {
            List<string> options = new List<string>();
            options.Add("none");
            int index = 0;
            for (int i = 0; i < LocalizationHolder.Prefab.dicts.Count; i++)
            {
                options.Add(LocalizationHolder.Prefab.dicts[i].key);
                if (LocalizationHolder.Prefab.dicts[i].key == Value)
                {
                    index = i + 1;
                }
            }
            int ind = EditorGUI.Popup(r, index, options.ToArray());
            if (ind == 0)
                return "";
            return options[ind];
        }

        private bool EqualToString(string str, LocalizationString obj)
        {
            str = str.ToLower();
            if (obj.key.ToLower().Contains(str))
                return true;
            return false;
        }

        string SelectString(Rect r, string Value)
        {
            List<string> options = new List<string>();
            options.Add("none");
            int index = -1;
            int ind = 1;
            for (int i = 0; i < _dict.strings.Count; i++)
            {
                if (!string.IsNullOrEmpty(Value) && !EqualToString(Value, _dict.strings[i]))
                    continue;
                options.Add(_dict.strings[i].key);
                if (_dict.strings[i].key == Value)
                {
                    index = ind;
                }
                ind++;
            }
            options.Add("--create " + Value);
            int result = EditorGUI.Popup(r, index, options.ToArray());
            if (result == 0)
                return "";
            else if (result == options.Count - 1)
            {
                Localization.AddString(_dict.key, Value);
                Localization.GetStringAsObject(_dict.key, Value).Repair();
                return Value;
            }
            if (result == -1)
                return Value;
            return options[result];
        }

        void DrawDictKey(Rect rect, string Value)
        {
            EditorGUI.LabelField(rect, "Value:" + Value);
        }




    }
}
