//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaterialUI
{
    [Serializable]
    public class OptionDataList
    {
        [SerializeField]
		private ImageDataType m_ImageType = ImageDataType.VectorImage;
		public ImageDataType imageType
		{
			get { return m_ImageType; }
			set { m_ImageType = value; }
		}

        [SerializeField]
        private List<OptionData> m_Options = new List<OptionData>();
		public List<OptionData> options
		{
			get { return m_Options; }
			set { m_Options = value; }
		}
    }

    [Serializable]
    public class OptionData
    {
        [SerializeField]
        private string m_Text;
		public string text
		{
			get { return m_Text; }
			set { m_Text = value; }
		}

        [SerializeField]
        private ImageData m_ImageData;
        public ImageData imageData
        {
            get { return m_ImageData; }
            set { m_ImageData = value; }
        }

		private Action m_OnOptionSelected;
		public Action onOptionSelected
		{
			get { return m_OnOptionSelected; }
			set { m_OnOptionSelected = value; }
		}

        public OptionData() { }

		public OptionData(string text, ImageData imageData, Action onOptionSelected = null)
        {
            m_Text = text;
            m_ImageData = imageData;
			m_OnOptionSelected = onOptionSelected;
        }
    }
}