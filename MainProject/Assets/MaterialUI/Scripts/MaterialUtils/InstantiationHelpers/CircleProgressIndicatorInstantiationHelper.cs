//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    public class CircleProgressIndicatorInstantiationHelper : InstantiationHelper
    {
        [SerializeField]
        private GameObject m_Shadow;

        [SerializeField]
        private GameObject m_BackgroundImage;

        [SerializeField]
        private GameObject m_Label;

        [SerializeField]
        private LayoutGroup m_LayoutGroup;

        [SerializeField]
        private ProgressIndicator m_ProgressIndicator;

        public override void HelpInstantiate(params InstantiationOptions[] options)
        {
            if (!options.Contains(InstantiationOptions.Raised))
            {
                DestroyImmediate(m_Shadow);
                DestroyImmediate(m_BackgroundImage);
            }

            if (!options.Contains(InstantiationOptions.Label))
            {
                DestroyImmediate(m_Label);
            }

            if (!options.Contains(InstantiationOptions.HasLayoutHorizontal) && !options.Contains(InstantiationOptions.HasLayoutVertical))
            {
                this.transform.SetParent(m_LayoutGroup.transform.parent, true);
                GameObject.DestroyImmediate(m_LayoutGroup.gameObject);
                m_ProgressIndicator.baseObjectOverride = null;
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

                if (options.Contains(InstantiationOptions.Raised))
                {
                    if (options.Contains(InstantiationOptions.HasLayoutHorizontal))
                    {
                        ((HorizontalLayoutGroup)m_LayoutGroup).spacing = 16f;
                    }
                    else if (options.Contains(InstantiationOptions.HasLayoutVertical))
                    {
                        ((VerticalLayoutGroup)m_LayoutGroup).spacing = 16f;
                    }
                }
            }

            base.HelpInstantiate(options);
        }
    }
}