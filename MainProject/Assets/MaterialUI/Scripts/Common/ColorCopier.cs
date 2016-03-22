//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MaterialUI
{
    [ExecuteInEditMode]
    [AddComponentMenu("MaterialUI/Color Copier", 100)]
    public class ColorCopier : UIBehaviour
    {
        [SerializeField]
        private Graphic m_SourceGraphic;
		public Graphic sourceGraphic
		{
			get { return m_SourceGraphic; }
			set
			{
				m_SourceGraphic = value;
				UpdateColor();
			}
		}

        [SerializeField]
        private Graphic m_DestinationGraphic;
		public Graphic destinationGraphic
		{
			get { return m_DestinationGraphic; }
			set
			{
				m_DestinationGraphic = value;
				UpdateColor();
			}
		}

        private Color m_LastColor;

        private void LateUpdate()
        {
            UpdateColor();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            UpdateColor();
        }
#endif

        public void UpdateColor()
        {
            if (sourceGraphic && destinationGraphic)
            {
                if (sourceGraphic.color != m_LastColor)
                {
                    destinationGraphic.color = sourceGraphic.color;
                    m_LastColor = sourceGraphic.color;
                }
            }
        }
    }
}