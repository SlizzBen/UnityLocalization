using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlizzLoc
{
    [RequireComponent(typeof(TextMesh))]
    public class U3DText : MonoBehaviour
    {

        public string list;
        public string key;

        TextMesh txt_obj;

        private void Awake()
        {
            Localization.Instance.OnChangeLanguage += UpdateText;
        }




        void UpdateText()
        {
            if (txt_obj == null)
                txt_obj = GetComponent<TextMesh>();
            txt_obj.text = Localization.GetString(list, key);
        }



    }
}

