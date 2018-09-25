using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace SlizzLoc
{
    [CustomEditor(typeof(UGUIText))]
    public class LocUGUIText_editor : Editor
    {

        string dict_key = "";
        string string_key = "";

        private void OnEnable()
        {
            UGUIText main = (UGUIText)target;
            dict_key = main.list;
            string_key = main.key;
        }

        public override void OnInspectorGUI()
        {
            DrawListKey();
            DrawStringKey();

            UGUIText main = (UGUIText)target;
            main.list = dict_key;
            main.key = string_key;
        }

        void DrawListKey()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("List key", GUILayout.Width(70));
            GUILayout.Label(dict_key, GUILayout.Width(70));

            List<string> allowed_dict = new List<string>();
            for (int i = 0; i < Localization.Instance.dicts.Count; i++)
            {
                allowed_dict.Add(Localization.Instance.dicts[i].key);
            }

            int index = EditorGUILayout.Popup(-1, allowed_dict.ToArray(), GUILayout.Width(100));
            if (index != -1)
            {
                dict_key = allowed_dict[index];
            }
            GUILayout.EndHorizontal();
        }

        void DrawStringKey()
        {
            if (dict_key == "")
                return;
            var d = Localization.Instance.GetDictByKey(dict_key);
            if (d == null)
                return;
            GUILayout.BeginHorizontal();
            GUILayout.Label("String key", GUILayout.Width(70));
            GUILayout.Label(string_key, GUILayout.Width(70));

            List<string> allowed_dict = new List<string>();
            for (int i = 0; i < d.strings.Count; i++)
            {
                allowed_dict.Add(d.strings[i].key);
            }

            int index = EditorGUILayout.Popup(-1, allowed_dict.ToArray(), GUILayout.Width(100));
            if (index != -1)
            {
                string_key = allowed_dict[index];
                Undo.RecordObject(target, "Change string");
            }
            GUILayout.EndHorizontal();
        }

    }

}
