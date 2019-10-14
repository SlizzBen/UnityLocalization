using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SlizzLoc
{
    [RequireComponent(typeof(TextMesh))]
    public class U3DText : LocObject
    {
        TextMesh txt_obj;

        protected override void OnAwake()
        {
            txt_obj = GetComponent<TextMesh>();
        }

        public override void UpdateText()
        {
            if(txt_obj != null)
            {
                txt_obj.text = Localization.GetString(GroupKey, StringKey);
            }
        }


    }
}

