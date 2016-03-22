//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MaterialUI
{
    public class PanelInstantiationHelper : InstantiationHelper
    {
        [SerializeField]
        private RectTransform m_RectTransform;

        [SerializeField]
        private ContentSizeFitter m_SizeFitter;

        [SerializeField]
        private LayoutGroup m_TopLayoutGroup;

        [SerializeField]
        private RectTransform m_BottomRectTransform;

        private LayoutGroup m_BottomLayoutGroup;

        public override void HelpInstantiate(params InstantiationOptions[] options)
        {
            if (!options.Contains(InstantiationOptions.HasLayoutHorizontal) && !options.Contains(InstantiationOptions.HasLayoutVertical))
            {
                DestroyImmediate(m_TopLayoutGroup);
                m_BottomRectTransform.anchorMin = Vector2.zero;
                m_BottomRectTransform.anchorMax = Vector2.one;
                m_BottomRectTransform.sizeDelta = Vector2.zero;
                m_BottomRectTransform.anchoredPosition = Vector2.zero;
            }
            else
            {
                if (options.Contains(InstantiationOptions.HasLayoutHorizontal))
                {
                    GameObject go = m_TopLayoutGroup.gameObject;
                    DestroyImmediate(m_TopLayoutGroup);
                    m_TopLayoutGroup = go.AddComponent<HorizontalLayoutGroup>();
                    m_TopLayoutGroup.childAlignment = TextAnchor.MiddleCenter;

                    m_BottomLayoutGroup = m_BottomRectTransform.gameObject.AddComponent<HorizontalLayoutGroup>();
                    m_BottomLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
                    ((HorizontalLayoutGroup)m_BottomLayoutGroup).childForceExpandWidth = false;
                    ((HorizontalLayoutGroup)m_BottomLayoutGroup).childForceExpandHeight = false;
                }
                else
                {
                    m_BottomLayoutGroup = m_BottomRectTransform.gameObject.AddComponent<VerticalLayoutGroup>();
                    m_BottomLayoutGroup.childAlignment = TextAnchor.MiddleCenter;
                    ((VerticalLayoutGroup)m_BottomLayoutGroup).childForceExpandWidth = false;
                    ((VerticalLayoutGroup)m_BottomLayoutGroup).childForceExpandHeight = false;
                }
            }

            if (!options.Contains(InstantiationOptions.Fitted))
            {
                DestroyImmediate(m_SizeFitter);
                m_RectTransform.sizeDelta = new Vector2(300f, 300f);
                m_RectTransform.anchoredPosition = Vector2.zero;
            }

            if (options.Contains(InstantiationOptions.HasContent))
            {
                m_RectTransform.anchorMin = new Vector2(0f, 0f);
                m_RectTransform.anchorMax = new Vector2(1f, 1f);
                m_RectTransform.sizeDelta = new Vector2(-48f, -48f);
                m_RectTransform.anchoredPosition = Vector2.zero;
            }

#if UNITY_EDITOR
            Selection.activeGameObject = m_BottomRectTransform.gameObject;
#endif

            base.HelpInstantiate(options);
        }
    }
}