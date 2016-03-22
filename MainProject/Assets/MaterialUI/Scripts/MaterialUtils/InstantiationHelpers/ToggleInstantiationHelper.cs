//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    public class ToggleInstantiationHelper : InstantiationHelper
    {
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

			base.HelpInstantiate(options);

#if UNITY_EDITOR
			m_ToggleBase.EditorValidate();
#endif
		}
    }
}