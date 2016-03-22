//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;

namespace MaterialUI
{
    public class InputFieldInstantiationHelper : InstantiationHelper
    {
        [SerializeField]
        private MaterialInputField m_MaterialInputField;

        [SerializeField]
        private GameObject m_Icon;

        [SerializeField]
        private GameObject m_ClearButton;

        public override void HelpInstantiate(params InstantiationOptions[] options)
        {
            if (!options.Contains(InstantiationOptions.Icon))
            {
                DestroyImmediate(m_Icon);
                m_MaterialInputField.leftContentTransform = null;
                m_MaterialInputField.leftContentGraphic = null;
            }

            if (!options.Contains(InstantiationOptions.HasContent)) // Clear button
            {
                DestroyImmediate(m_ClearButton);
                m_MaterialInputField.rightContentTransform = null;
                m_MaterialInputField.rightContentGraphic = null;
            }

            m_MaterialInputField.CalculateLayoutInputHorizontal();
            m_MaterialInputField.SetLayoutHorizontal();
            m_MaterialInputField.CalculateLayoutInputVertical();
            m_MaterialInputField.SetLayoutVertical();

            base.HelpInstantiate(options);
        }
    }
}