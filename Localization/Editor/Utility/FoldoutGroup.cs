using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SlizzLoc
{
    public class FoldoutGroup
    {
        Dictionary<string, bool> foldoutDict = new Dictionary<string, bool>();

        public bool GetFoldout(string obj)
        {
            if (foldoutDict.ContainsKey(obj))
            {
                return foldoutDict[obj];
            }
            return false;
        }

        public void SetFoldout(string obj, bool b)
        {
            if (!foldoutDict.ContainsKey(obj))
            {
                foldoutDict.Add(obj, b);
                return;
            }
            foldoutDict[obj] = b;
        }

        public bool GUIFoldoutGroup(string key, string name)
        {
            bool oldValue = GetFoldout(key);
            bool newValue = EditorGUILayout.Foldout(oldValue, name, true);
            if (oldValue != newValue)
            {
                SetFoldout(key, newValue);
            }
            return newValue;
        }
    }
}
