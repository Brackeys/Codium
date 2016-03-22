//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    public class DividerInstantiationHelper : InstantiationHelper
    {
        [SerializeField]
        private LayoutElement m_LayoutElement;

        [SerializeField]
        private Image m_Image;

        public override void HelpInstantiate(params InstantiationOptions[] options)
        {
            if (options.Contains(InstantiationOptions.Light))
            {
                m_Image.color = MaterialColor.dividerLight;
            }

            if (options.Contains(InstantiationOptions.Vertical))
            {
                m_LayoutElement.minHeight = -1f;
                m_LayoutElement.minWidth = 1f;

                m_Image.rectTransform.anchorMin = new Vector2(0.5f, 0f);
                m_Image.rectTransform.anchorMax = new Vector2(0.5f, 1f);
                m_Image.rectTransform.anchoredPosition = new Vector2(0f, 0f);
                m_Image.rectTransform.sizeDelta = new Vector2(1f, 0f);
            }
            else
            {
                m_Image.rectTransform.anchoredPosition = new Vector2(0f, 0f);
                m_Image.rectTransform.sizeDelta = new Vector2(0f, 1f);
            }

            base.HelpInstantiate(options);
        }
    }
}