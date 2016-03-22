//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    public class MaterialSliderInstantiationHelper : InstantiationHelper
    {
        [SerializeField]
        private MaterialSlider m_MaterialSlider;

        [SerializeField]
        private Slider m_Slider;

        [SerializeField]
        private GameObject m_LeftLabel;

        [SerializeField]
        private GameObject m_LeftIcon;

        [SerializeField]
        private GameObject m_RightLabel;

        [SerializeField]
        private GameObject m_RightInputField;

        [SerializeField]
        private GameObject m_LeftContent;

        [SerializeField]
        private GameObject m_RightContent;

        public override void HelpInstantiate(params InstantiationOptions[] options)
        {
            bool destroyLeft = true;
            bool destroyRight = true;

            if (options.Contains(InstantiationOptions.Discrete))
            {
                m_Slider.wholeNumbers = true;
            }

            if (!options.Contains(InstantiationOptions.Icon))
            {
                DestroyImmediate(m_LeftIcon);
            }
            else
            {
                destroyLeft = false;
            }

            if (!options.Contains(InstantiationOptions.Label))
            {
                DestroyImmediate(m_LeftLabel);
            }
            else
            {
                destroyLeft = false;
            }

            if (!options.Contains(InstantiationOptions.HasContent))
            {
                DestroyImmediate(m_RightLabel);
                m_MaterialSlider.valueText = null;
            }
            else
            {
                destroyRight = false;
            }

            if (!options.Contains(InstantiationOptions.HasInputField))
            {
                DestroyImmediate(m_RightInputField);
                m_MaterialSlider.inputField = null;
            }
            else
            {
                destroyRight = false;
                m_MaterialSlider.lowRightDisabledOpacity = false;
            }

            if (destroyLeft)
            {
                DestroyImmediate(m_LeftContent);
            }

            if (destroyRight)
            {
                DestroyImmediate(m_RightContent);
            }

            if (transform.parent.GetComponent<ILayoutController>() != null)
            {
                m_MaterialSlider.hasManualPreferredWidth = true;
                m_MaterialSlider.manualPreferredWidth = 200;
            }

            m_MaterialSlider.SetLayoutHorizontal();

            base.HelpInstantiate(options);
        }
    }
}