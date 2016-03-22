//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/Rect Transform Snap", 50)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformSnap : MonoBehaviour, ILayoutElement, ILayoutSelfController
    {
        [SerializeField]
        private RectTransform m_RectTransform;
		public RectTransform rectTransform
		{
			get
			{
				if (!m_RectTransform)
				{
					m_RectTransform = transform as RectTransform;
				}
				return m_RectTransform;
			}
		}

        [SerializeField]
        private RectTransform m_SourceRectTransform;
		public RectTransform sourceRectTransform
		{
			get { return m_SourceRectTransform; }
			set
			{
				m_SourceRectTransform = value;
				SetLayoutDirty();
			}
		}

        [SerializeField]
        private Vector2 m_Padding;
		public Vector2 padding
		{
			get { return m_Padding; }
			set
			{
				m_Padding = value;
				SetLayoutDirty();
			}
		}

        [SerializeField]
        private Vector2 m_Offset;
		public Vector2 offset
		{
			get { return m_Offset; }
			set
			{
				m_Offset = value;
				SetLayoutDirty();
			}
		}

        [SerializeField]
        private bool m_ValuesArePercentage;
		public bool valuesArePercentage
		{
			get { return m_ValuesArePercentage; }
			set
			{
				m_ValuesArePercentage = value;
				SetLayoutDirty();
			}
		}

        [SerializeField]
        private Vector2 m_PaddingPercent = new Vector2(100, 100);
		public Vector2 paddingPercent
		{
			get { return m_PaddingPercent; }
			set
			{
				m_PaddingPercent = value;
				SetLayoutDirty();
			}
		}

        [SerializeField]
        private Vector2 m_OffsetPercent;
		public Vector2 offsetPercent
		{
			get { return m_OffsetPercent; }
			set
			{
				m_OffsetPercent = value;
				SetLayoutDirty();
			}
		}
		
        [SerializeField]
        private bool m_SnapEveryFrame = true;
		public bool snapEveryFrame
		{
			get { return m_SnapEveryFrame; }
			set { m_SnapEveryFrame = value; }
		}
		
        [SerializeField]
        private bool m_SnapWidth = true;
		public bool snapWidth
		{
			get { return m_SnapWidth; }
			set
			{
				m_SnapWidth = value;
				SetLayoutDirty();
			}
		}

        [SerializeField]
        private bool m_SnapHeight = true;
		public bool snapHeight
		{
			get { return m_SnapHeight; }
			set
			{
				m_SnapHeight = value;
				SetLayoutDirty();
			}
		}

        [SerializeField]
        private bool m_SnapPositionX = true;
		public bool snapPositionX
		{
			get { return m_SnapPositionX; }
			set
			{
				m_SnapPositionX = value;
				SetLayoutDirty();
			}
		}

        [SerializeField]
        private bool m_SnapPositionY = true;
		public bool snapPositionY
		{
			get { return m_SnapPositionY; }
			set
			{
				m_SnapPositionY = value;
				SetLayoutDirty();
			}
		}

        private Rect m_LastRect;
        private Rect m_LayoutRect;

		private DrivenRectTransformTracker m_Tracker = new DrivenRectTransformTracker();

        void Awake()
        {
            SetLayoutDirty();
        }

        void OnEnable()
        {
            SetLayoutDirty();
        }

        void OnDisable()
        {
            m_Tracker.Clear();
            SetLayoutDirty();
        }

        void OnValidate()
        {
            SetLayoutDirty();
        }

        void LateUpdate()
        {
            if (!m_SourceRectTransform) return;

            if (m_SnapEveryFrame)
            {
				Rect rect = new Rect(m_SourceRectTransform.position, m_SourceRectTransform.GetProperSize());
             	if (m_LastRect != rect)
                {
                    m_LastRect = rect;

                    CalculateLayoutInputHorizontal();
                    SetLayoutHorizontal();
                    CalculateLayoutInputVertical();
                    SetLayoutVertical();
                }
            }
        }

        public void SetLayoutDirty()
        {
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        public void CalculateLayoutInputHorizontal()
        {
            if (!m_SourceRectTransform) return;

            if (m_SnapPositionX)
            {
                float sourcePosX = m_SourceRectTransform.position.x;

                Vector2 tempVector2 = m_LayoutRect.position;

                if (m_ValuesArePercentage)
                {
                    tempVector2.x = sourcePosX * m_OffsetPercent.x * 0.01f;
                }
                else
                {
                    tempVector2.x = sourcePosX + m_Offset.x;
                }

                m_LayoutRect.position = tempVector2;
            }

            if (m_SnapWidth)
            {
                float sourceWidth = m_SourceRectTransform.GetProperSize().x;

                Rect tempRect = m_LayoutRect;

                if (m_ValuesArePercentage)
                {
                    tempRect.width = sourceWidth * m_PaddingPercent.x * 0.01f;
                }
                else
                {
                    tempRect.width = sourceWidth + m_Padding.x;
                }

                m_LayoutRect = tempRect;
            }
        }

        public void CalculateLayoutInputVertical()
        {
            if (!m_SourceRectTransform) return;

            if (m_SnapPositionY)
            {
                float sourcePosY = m_SourceRectTransform.position.y;

                Vector2 tempVector2 = m_LayoutRect.position;

                if (m_ValuesArePercentage)
                {
                    tempVector2.y = sourcePosY * m_OffsetPercent.y * 0.01f;
                }
                else
                {
                    tempVector2.y = sourcePosY + m_Offset.y;
                }

                m_LayoutRect.position = tempVector2;
            }

            if (m_SnapHeight)
            {
                float sourceHeight = m_SourceRectTransform.GetProperSize().y;

                Rect tempRect = m_LayoutRect;

                if (m_ValuesArePercentage)
                {
                    tempRect.height = sourceHeight * m_PaddingPercent.y * 0.01f;
                }
                else
                {
                    tempRect.height = sourceHeight + m_Padding.y;
                }

                m_LayoutRect = tempRect;
            }
        }

        public void SetLayoutHorizontal()
        {
            m_Tracker.Clear();

            if (!m_SourceRectTransform) return;

            if (m_SnapPositionX)
            {
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionX);

                Vector3 tempVector3 = rectTransform.position;
                tempVector3.x = m_LayoutRect.position.x;
                rectTransform.position = tempVector3;
            }

            if (m_SnapWidth)
            {
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);

                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_LayoutRect.width);
            }
        }

        public void SetLayoutVertical()
        {
            if (!m_SourceRectTransform) return;

            if (m_SnapPositionY)
            {
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.AnchoredPositionY);

                Vector3 tempVector3 = rectTransform.position;
                tempVector3.y = m_LayoutRect.position.y;
                rectTransform.position = tempVector3;
            }

            if (m_SnapHeight)
            {
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);

                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_LayoutRect.height);
            }
        }

        public float minWidth { get { return -1; }}
        public float preferredWidth { get { return m_LayoutRect.width; } }
        public float flexibleWidth { get { return -1; } }
        public float minHeight { get { return -1; } }
        public float preferredHeight { get { return m_LayoutRect.height; } }
        public float flexibleHeight { get { return -1; } }
        public int layoutPriority { get { return 0; } }
    }
}