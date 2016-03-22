//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;

namespace MaterialUI
{
    public class ButtonRoundInstantiationHelper : InstantiationHelper
    {
        [SerializeField]
        private MaterialButton m_Button;

        [SerializeField]
        private MaterialDropdown m_Dropdown;

        [SerializeField]
        private RectTransform m_RectTransform;

        [SerializeField]
        private RectTransform m_ImageRectTransform;

        [SerializeField]
        private RectTransform m_Shadows;

        public override void HelpInstantiate(params InstantiationOptions[] options)
        {
            m_Button.isCircularButton = true;

            if (options.Contains(InstantiationOptions.Mini))
            {
                m_Button.contentPadding = new Vector2(16, 16);
                m_Button.contentPadding = new Vector2(16, 16);
                m_Shadows.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 180);
                m_Shadows.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 180);
                m_RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 40);
                m_RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);
                m_ImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 40);
                m_ImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);
            }

            if (!options.Contains(InstantiationOptions.Raised))
            {
                m_Button.isRaisedButton = true;
                m_Button.Convert(true);
            }
            else
            {
                m_Button.isRaisedButton = true;
            }

            if (!options.Contains(InstantiationOptions.HasDropdown))
            {
                DestroyImmediate(m_Dropdown);
                m_Button.buttonObject.onClick = null;
            }

            base.HelpInstantiate(options);
        }
    }
}