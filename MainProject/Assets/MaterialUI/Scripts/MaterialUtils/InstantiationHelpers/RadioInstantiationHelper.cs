//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    public class RadioInstantiationHelper : InstantiationHelper
    {
        [SerializeField]
        private MaterialRadioGroup m_Group;

        [SerializeField]
        private ToggleBase m_ToggleBase;

        [SerializeField]
        private Text m_Label;

        [SerializeField]
        private VectorImage m_Icon;

        public override void HelpInstantiate(params InstantiationOptions[] options)
        {
            if (options.Contains(InstantiationOptions.Label))
            {
                DestroyImmediate(m_Icon.gameObject);
                m_ToggleBase.graphic = m_Label;
                m_ToggleBase.graphicOffColor = MaterialColor.textDark;
            }
            else if (options.Contains(InstantiationOptions.Icon))
            {
                DestroyImmediate(m_Label.gameObject);
                m_ToggleBase.graphic = m_Icon;
                m_ToggleBase.graphicOffColor = MaterialColor.iconDark;
            }

            for (int i = 0; i < 2; i++)
            {
                RectTransform instance = (RectTransform)Instantiate(m_ToggleBase.gameObject).transform;
                instance.SetParent(m_ToggleBase.transform.parent);
                instance.localScale = Vector3.one;
                instance.localEulerAngles = Vector3.zero;

                instance.name = "RadioButton " + (i + 2);
                instance.GetComponent<Toggle>().isOn = false;
            }

            base.HelpInstantiate(options);
        }
    }
}