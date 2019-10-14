using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlizzLoc
{
    [RequireComponent(typeof(Text))]
    public class UGUIText : LocObject
    {
        Text txt_obj;

        protected override void OnAwake()
        {
            txt_obj = GetComponent<Text>();
        }

        public override void UpdateText()
        {
            if (txt_obj != null)
            {
                txt_obj.text = Localization.GetString(GroupKey, StringKey);
            }
        }
    }
}

