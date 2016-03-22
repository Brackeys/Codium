//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    public class ButtonRectInstantiationHelper : InstantiationHelper
    {
        [SerializeField]
        private MaterialButton m_Button;

        [SerializeField]
        private MaterialDropdown m_Dropdown;

        [SerializeField]
        private RectTransform m_RectTransform;

        [SerializeField]
        private HorizontalLayoutGroup m_Content;

        [SerializeField]
        private RectTransform m_Text;

        [SerializeField]
        private Graphic m_Icon;

        [SerializeField]
        private VectorImageData m_IconData;

        public override void HelpInstantiate(params InstantiationOptions[] options)
        {
            m_Button.isCircularButton = false;

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
                m_Icon.rectTransform.SetAsFirstSibling();
                m_Icon.SetImage(m_IconData);
                RectOffset offset = m_Content.padding;
                offset.right = 0;
                m_Content.padding = offset;
                m_Button.text.text = "BUTTON";
            }
            else
            {
                m_Button.icon = null;

                m_Button.fitWidthToContent = false;
                m_Content.childAlignment = TextAnchor.MiddleLeft;
                m_Content.padding.top = 0;
                m_Content.padding.bottom = 0;
                RectTransform contentTransform = (RectTransform)m_Content.transform;
                contentTransform.sizeDelta = new Vector2(-30f, contentTransform.sizeDelta.y);
                contentTransform.anchoredPosition = new Vector2(0f, contentTransform.anchoredPosition.y);
                m_Button.rectTransform.sizeDelta = new Vector2(134, m_Button.rectTransform.sizeDelta.y);
                m_Icon.gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
                m_Icon.rectTransform.anchorMin = new Vector2(1f, 0.5f);
                m_Icon.rectTransform.anchorMax = new Vector2(1f, 0.5f);
                m_Icon.rectTransform.anchoredPosition = new Vector2(-12f, 0f);
                m_Icon.rectTransform.sizeDelta = new Vector2(24f, 24f);
                gameObject.AddComponent<LayoutElement>().preferredWidth = 134;
            }

            if (!options.Contains(InstantiationOptions.HasContent))
            {
                m_Button.contentRectTransform = m_Text;
                m_Text.SetParent(m_RectTransform);
                m_Text.anchorMin = Vector2.zero;
                m_Text.anchorMax = Vector2.one;
                DestroyImmediate(m_Content.gameObject);
                m_Button.icon = null;
                m_Button.SetLayoutDirty();
            }

            base.HelpInstantiate(options);
        }
    }
}