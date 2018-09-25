using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlizzLoc
{
    [RequireComponent(typeof(Text))]
    public class UGUIText : MonoBehaviour
    {

        public string list;
        public string key;

        Text txt_obj;

        private void Awake()
        {
            Localization.Instance.OnChangeLanguage += UpdateText;
        }




        void UpdateText()
        {
            if (txt_obj == null)
                txt_obj = GetComponent<Text>();
            txt_obj.text = Localization.GetString(list, key);
        }



    }
}

