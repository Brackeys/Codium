//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    public class EmptyUIObjectInstantiationHelper : InstantiationHelper
    {
        [SerializeField]
        private RectTransform m_RectTransform;

        [SerializeField]
        private ContentSizeFitter m_SizeFitter;

        [SerializeField]
        private LayoutGroup m_LayoutGroup;

        public override void HelpInstantiate(params InstantiationOptions[] options)
        {
            if (!options.Contains(InstantiationOptions.HasLayoutHorizontal) && !options.Contains(InstantiationOptions.HasLayoutVertical))
            {
                DestroyImmediate(m_LayoutGroup);
            }
            else
            {
                if (options.Contains(InstantiationOptions.HasLayoutHorizontal))
                {
                    GameObject go = m_LayoutGroup.gameObject;
                    DestroyImmediate(m_LayoutGroup);
                    m_LayoutGroup = go.AddComponent<HorizontalLayoutGroup>();
                    m_LayoutGroup.childAlignment = TextAnchor.MiddleCenter;
                    ((HorizontalLayoutGroup)m_LayoutGroup).childForceExpandWidth = false;
                    ((HorizontalLayoutGroup)m_LayoutGroup).childForceExpandHeight = false;
                }
                else
                {
                    m_LayoutGroup = m_RectTransform.gameObject.GetAddComponent<VerticalLayoutGroup>();
                    m_LayoutGroup.childAlignment = TextAnchor.MiddleCenter;
                    ((VerticalLayoutGroup)m_LayoutGroup).childForceExpandWidth = false;
                    ((VerticalLayoutGroup)m_LayoutGroup).childForceExpandHeight = false;
                }
            }

            if (!options.Contains(InstantiationOptions.Fitted))
            {
                DestroyImmediate(m_SizeFitter);
                m_RectTransform.sizeDelta = new Vector2(100f, 100f);
                m_RectTransform.anchoredPosition = Vector2.zero;
            }

            if (options.Contains(InstantiationOptions.HasContent))
            {
                m_RectTransform.anchorMin = new Vector2(0f, 0f);
                m_RectTransform.anchorMax = new Vector2(1f, 1f);
                m_RectTransform.sizeDelta = Vector2.zero;
                m_RectTransform.anchoredPosition = Vector2.zero;
            }

            base.HelpInstantiate(options);
        }
    }
}