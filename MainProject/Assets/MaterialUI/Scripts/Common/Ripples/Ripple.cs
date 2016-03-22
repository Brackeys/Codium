//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    public class Ripple : MonoBehaviour
    {
		private int m_Id;
		public int id
		{
			get { return m_Id; }
			set { m_Id = value; }
		}

        private RippleData m_RippleData;
		public RippleData rippleData
		{
			get { return m_RippleData; }
		}

        private RectTransform m_RectTransform;
		public RectTransform rectTransform
		{
			get { return m_RectTransform; }
		}
		
        private VectorImage m_Image;
		public VectorImage image
		{
			get { return m_Image; }
		}

        private CanvasGroup m_CanvasGroup;
		public CanvasGroup canvasGroup
		{
			get { return m_CanvasGroup; }
		}

        private IRippleCreator m_RippleCreator;
		private RectTransform m_RippleParent;

        private float m_CurrentAlpha;
        private float m_CurrentSize;
        private Vector3 m_CurrentPosition;

        private float m_ZPosition;
        private bool m_Oscillate;
        
		private float m_AnimationDuration;
        private float m_AnimStartTime;
        private float m_AnimDeltaTime;
		private int m_State;

        public void Create(int id, VectorImageData imageData)
        {
            if (m_Id != 0)
            {
                Debug.Log("Cannot Setup a Ripple more than once");
                return;
            }

            m_Id = id;
            m_RectTransform = GetComponent<RectTransform>();
            m_Image = GetComponent<VectorImage>();
            m_CanvasGroup = GetComponent<CanvasGroup>();
            m_Image.vectorImageData = imageData;
            m_CanvasGroup.blocksRaycasts = false;
            m_CanvasGroup.interactable = false;
            gameObject.AddComponent<LayoutElement>().ignoreLayout = true;
        }

        public void ClearData()
        {
            m_RippleData = null;
        }

        public void Setup(RippleData data, Vector2 positon, IRippleCreator creator, bool oscillate = false)
        {
            m_RippleData = data;
            m_RippleParent = (RectTransform)data.RippleParent;
            rectTransform.SetParent(m_RippleParent);
            rectTransform.SetSiblingIndex(0);
            m_Oscillate = oscillate;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.position = CalculatePosition(positon);
            image.color = data.Color;
            canvasGroup.alpha = data.StartAlpha;
            m_AnimationDuration = 4f / data.Speed;
            m_RippleCreator = creator;
            if (data.PlaceRippleBehind)
            {
                int index = m_RippleParent.GetSiblingIndex();
                rectTransform.SetParent(m_RippleParent.parent);
                rectTransform.SetSiblingIndex(index);
            }
        }

        private Vector3 CalculatePosition(Vector2 position)
        {
            m_ZPosition = m_RippleParent.position.z * 0.99f;

            RenderMode renderMode = GetComponentInParent<Canvas>().renderMode;

            if (renderMode == RenderMode.ScreenSpaceOverlay)
            {
                return new Vector3(position.x, position.y, m_ZPosition);
            }
            else
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ZPosition));
                return new Vector3(pos.x, pos.y, m_ZPosition);
            }
        }

        private float CalculateSize()
        {
            float size;

            if (rippleData.AutoSize)
            {
                float x = rectTransform.parent.GetComponent<RectTransform>().GetProperSize().x;
                float y = rectTransform.parent.GetComponent<RectTransform>().GetProperSize().y;

                if (rippleData.SizeMode == RippleData.SizeModeType.FillRect)
                {
                    x *= x;
                    y *= y;

                    x *= (rippleData.SizePercent / 100f);
                    y *= (rippleData.SizePercent / 100f);

                    size = Mathf.Sqrt(x + y);

                }
                else
                {
                    size = Mathf.Max(x, y);
                }

            }
            else
            {
                size = rippleData.ManualSize;
            }

            if (m_Oscillate)
            {
                size *= 0.75f;
            }

            return size;
        }

        public void Expand()
        {
            m_RippleCreator.OnCreateRipple();
            m_CurrentAlpha = canvasGroup.alpha;
            m_CurrentSize = rectTransform.rect.width;
            m_CurrentPosition = rectTransform.position;

            m_AnimStartTime = Time.realtimeSinceStartup;
            m_State = 1;
        }

        public void Contract()
        {
            m_CurrentAlpha = canvasGroup.alpha;
            m_CurrentSize = rectTransform.rect.width;
            m_CurrentPosition = rectTransform.position;
            m_AnimStartTime = Time.realtimeSinceStartup;
            m_State = 2;
        }

        void Update()
        {
            m_AnimDeltaTime = Time.realtimeSinceStartup - m_AnimStartTime;

            if (m_State == 1)
            {
                if (m_AnimDeltaTime <= m_AnimationDuration)
                {
                    canvasGroup.alpha = Tween.QuintOut(m_CurrentAlpha, rippleData.EndAlpha, m_AnimDeltaTime, m_AnimationDuration);
                    float size = Tween.QuintOut(m_CurrentSize, CalculateSize(), m_AnimDeltaTime, m_AnimationDuration);
                    rectTransform.sizeDelta = new Vector2(size, size);

                    if (rippleData.MoveTowardCenter)
                    {
                        Vector3 parentPosition = m_RippleParent.GetPositionRegardlessOfPivot();
                        rectTransform.position = Tween.QuintOut(m_CurrentPosition, new Vector3(parentPosition.x, parentPosition.y, m_ZPosition), m_AnimDeltaTime, m_AnimationDuration);
                    }
                }
                else
                {
                    if (m_Oscillate)
                    {
                        m_State = 3;
                        m_AnimStartTime = Time.realtimeSinceStartup;
                        m_CurrentSize = rectTransform.rect.width;
                        m_CurrentSize *= 0.95f;
                    }
                    else
                    {
                        m_State = 0;
                    }
                }
            }
            else if (m_State == 2)
            {
                if (m_AnimDeltaTime <= m_AnimationDuration * 2f)
                {
                    canvasGroup.alpha = Tween.QuintOut(m_CurrentAlpha, 0f, m_AnimDeltaTime, m_AnimationDuration * 2f);
                    float size = Tween.QuintOut(m_CurrentSize, CalculateSize(), m_AnimDeltaTime, m_AnimationDuration);
                    rectTransform.sizeDelta = new Vector2(size, size);

                    if (rippleData.MoveTowardCenter)
                    {
                        Vector3 parentPosition = m_RippleParent.GetPositionRegardlessOfPivot();
                        rectTransform.position = Tween.QuintOut(m_CurrentPosition, new Vector3(parentPosition.x, parentPosition.y, m_ZPosition), m_AnimDeltaTime, m_AnimationDuration);
                    }
                }
                else
                {
                    m_State = 0;
                    m_RippleCreator.OnDestroyRipple();
                    RippleManager.instance.ReleaseRipple(this);
                }
            }
            else if (m_State == 3)
            {
                float size = Tween.Sin(m_CurrentSize, m_CurrentSize * 0.05f, m_AnimDeltaTime * 4);

                rectTransform.sizeDelta = new Vector2(size, size);
            }
        }
    }

    [Serializable]
    public class RippleData
    {
        public enum SizeModeType
        {
            FillRect,
            MatchSize
        }

        public bool AutoSize = true;
        public float SizePercent = 105f;
        public float ManualSize;
        public SizeModeType SizeMode = SizeModeType.FillRect;
        public float Speed = 8f;
        public Color Color = Color.black;
        public float StartAlpha = 0.3f;
        public float EndAlpha = 0.1f;
        public bool MoveTowardCenter = true;
        public Transform RippleParent;
        public bool PlaceRippleBehind;

        public RippleData Copy()
        {
            RippleData data = new RippleData();
            data.AutoSize = AutoSize;
            data.SizePercent = SizePercent;
            data.ManualSize = ManualSize;
            data.SizeMode = SizeMode;
            data.Speed = Speed;
            data.Color = Color;
            data.StartAlpha = StartAlpha;
            data.EndAlpha = EndAlpha;
            data.MoveTowardCenter = MoveTowardCenter;
            data.RippleParent = RippleParent;
            data.PlaceRippleBehind = PlaceRippleBehind;
            return data;
        }
    }
}