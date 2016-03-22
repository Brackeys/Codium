//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/Vertical Scroll Layout Element", 50)]
    public class VerticalScrollLayoutElement : MonoBehaviour, ILayoutElement
    {
        [SerializeField]
        private float m_MaxHeight;
		public float maxHeight
		{
			get { return m_MaxHeight; }
			set
			{
				m_MaxHeight = value;
				RefreshLayout();
			}
		}

        [SerializeField]
        private RectTransform m_ContentRectTransform;
		public RectTransform contentRectTransform
		{
			get { return m_ContentRectTransform; }
			set
			{
				m_ContentRectTransform = value;
				RefreshLayout();
			}
		}

        [SerializeField]
        private ScrollRect m_ScrollRect;
		public ScrollRect scrollRect
		{
			get { return m_ScrollRect; }
			set
			{
				m_ScrollRect = value;
				RefreshLayout();
			}
		}

        private RectTransform m_ScrollRectTransform;
		public RectTransform scrollRectTransform
		{
			get { return m_ScrollRectTransform; }
			set
			{
				m_ScrollRectTransform = value;
				RefreshLayout();
			}
		}

        [SerializeField]
        private Image m_ScrollHandleImage;
		public Image scrollHandleImage
		{
			get { return m_ScrollHandleImage; }
			set
			{
				m_ScrollHandleImage = value;
				RefreshLayout();
			}
		}

        [SerializeField]
        private ScrollRect.MovementType m_MovementTypeWhenScrollable;
		public ScrollRect.MovementType movementTypeWhenScrollable
		{
			get { return m_MovementTypeWhenScrollable; }
			set
			{
				m_MovementTypeWhenScrollable = value;
				RefreshLayout();
			}
		}

        [SerializeField]
        private Image[] m_ShowWhenScrollable;

        private bool m_ScrollEnabled;
		public bool scrollEnabled
		{
			get { return m_ScrollEnabled; }
		}

		private float m_Height;

        private void RefreshLayout()
        {
            if (!m_ScrollRect)
            {
                m_ScrollRect = GetComponent<ScrollRect>();
            }
            if (!m_ScrollRectTransform)
            {
                m_ScrollRectTransform = m_ScrollRect.GetComponent<RectTransform>();
            }

            LayoutRebuilder.MarkLayoutForRebuild(contentRectTransform);

            float tempHeight = LayoutUtility.GetPreferredHeight(contentRectTransform);

            if (tempHeight > m_MaxHeight)
            {
                m_Height = maxHeight;
                m_ScrollRect.movementType = movementTypeWhenScrollable;
                m_ScrollHandleImage.enabled = true;

                m_ScrollEnabled = true;

                for (int i = 0; i < m_ShowWhenScrollable.Length; i++)
                {
                    m_ShowWhenScrollable[i].enabled = true;
                }
            }
            else
            {
                m_Height = tempHeight;
                m_ScrollRect.movementType = ScrollRect.MovementType.Clamped;
                m_ScrollHandleImage.enabled = false;

                m_ScrollEnabled = false;

                for (int i = 0; i < m_ShowWhenScrollable.Length; i++)
                {
                    m_ShowWhenScrollable[i].enabled = false;
                }
            }

            m_ScrollRectTransform.sizeDelta = new Vector2(m_ScrollRectTransform.sizeDelta.x, m_Height);
        }

        public void CalculateLayoutInputHorizontal() { }

        public void CalculateLayoutInputVertical()
        {
            RefreshLayout();
        }

        public float minWidth { get { return -1; } }
        public float preferredWidth { get { return -1; } }
        public float flexibleWidth { get { return -1; } }
        public float minHeight { get { return m_Height; } }
        public float preferredHeight { get { return m_Height; } }
        public float flexibleHeight { get { return m_Height; } }
        public int layoutPriority { get { return 0; } }
    }
}
