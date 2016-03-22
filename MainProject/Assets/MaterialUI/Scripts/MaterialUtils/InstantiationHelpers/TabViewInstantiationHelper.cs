//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    public class TabViewInstantiationHelper : InstantiationHelper
    {
        [SerializeField]
        private RectTransform m_RectTransform;

        [SerializeField]
        private TabItem m_ItemTemplate;

        [SerializeField]
        private RectTransform m_TabBarRectTransform;

        [SerializeField]
        private RectTransform m_PagesRectTransform;

        [SerializeField]
        private RectTransform[] m_IconRectTransforms;

        [SerializeField]
        private RectTransform m_TextRectTransform;

        public override void HelpInstantiate(params InstantiationOptions[] options)
        {
            m_RectTransform.sizeDelta = Vector2.zero;
            m_RectTransform.anchoredPosition = Vector2.zero;

            if (!options.Contains(InstantiationOptions.Icon) || !options.Contains(InstantiationOptions.Label))
            {
                m_TabBarRectTransform.sizeDelta = new Vector2(m_TabBarRectTransform.sizeDelta.x, 48);

                if (!options.Contains(InstantiationOptions.Icon))
                {
                    m_ItemTemplate.itemIcon = null;

                    for (int i = 0; i < m_IconRectTransforms.Length; i++)
                    {
                        DestroyImmediate(m_IconRectTransforms[i].gameObject);
                    }

                    m_TextRectTransform.anchorMin = Vector2.zero;
                    m_TextRectTransform.anchorMax = Vector2.one;
                    m_TextRectTransform.anchoredPosition = Vector2.zero;
                    m_TextRectTransform.sizeDelta = Vector2.zero;
                    m_TextRectTransform.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
                }
                else
                {
                    m_ItemTemplate.itemText = null;
                    DestroyImmediate(m_TextRectTransform.gameObject);

                    for (int i = 0; i < m_IconRectTransforms.Length; i++)
                    {
                        m_IconRectTransforms[i].anchorMin = new Vector2(0.5f, 0.5f);
                        m_IconRectTransforms[i].anchorMax = new Vector2(0.5f, 0.5f);
                        m_IconRectTransforms[i].pivot = new Vector2(0.5f, 0.5f);
                        m_IconRectTransforms[i].anchoredPosition = Vector2.zero;
                        m_IconRectTransforms[i].sizeDelta = new Vector2(24, 24);
                    }
                }

                m_PagesRectTransform.sizeDelta = new Vector2(m_PagesRectTransform.sizeDelta.x, -48);
                m_PagesRectTransform.anchoredPosition = new Vector2(m_PagesRectTransform.anchoredPosition.x, -24f);
            }

            base.HelpInstantiate(options);
        }
    }
}