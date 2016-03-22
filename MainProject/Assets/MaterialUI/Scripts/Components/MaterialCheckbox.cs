//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    [ExecuteInEditMode]
    [AddComponentMenu("MaterialUI/Toggles/Material Checkbox", 100)]
    public class MaterialCheckbox : ToggleBase
    {
        [SerializeField]
        private Graphic m_CheckImage;
		public Graphic checkImage
		{
			get { return m_CheckImage; }
			set { m_CheckImage = value; }
		}

        [SerializeField]
        private Graphic m_FrameImage;
		public Graphic frameImage
		{
			get { return m_FrameImage; }
			set { m_FrameImage = value; }
		}

        private RectTransform m_CheckRectTransform;
        public RectTransform checkRectTransform
        {
            get
            {
                if (m_CheckRectTransform == null)
                {
					if (m_CheckImage != null)
					{
                    m_CheckRectTransform = (RectTransform)m_CheckImage.transform;
                }
                }
                return m_CheckRectTransform;
            }
        }

		private float m_CurrentCheckSize;
		private Color m_CurrentFrameColor;

		protected override void OnEnable()
        {
            base.OnEnable();
            m_CheckRectTransform = checkImage.GetComponent<RectTransform>();
        }

        public override void TurnOn()
        {
            m_CurrentCheckSize = checkImage.rectTransform.sizeDelta.x;
            m_CurrentColor = checkImage.color;
            m_CurrentFrameColor = frameImage.color;

            base.TurnOn();
        }

        public override void TurnOnInstant()
        {
            base.TurnOnInstant();

            if (m_Toggle.interactable)
            {
                AnimOnComplete();
            }

            checkRectTransform.sizeDelta = new Vector2(24, 24);
        }

        public override void TurnOff()
        {
            m_CurrentCheckSize = checkImage.rectTransform.sizeDelta.x;
            m_CurrentColor = checkImage.color;
            m_CurrentFrameColor = frameImage.color;

            base.TurnOff();
        }

        public override void TurnOffInstant()
        {
            base.TurnOffInstant();

            if (m_Toggle.interactable)
            {
                AnimOffComplete();
            }

            checkRectTransform.sizeDelta = Vector2.zero;
        }

        public override void Enable()
        {
            base.Enable();

            if (m_Toggle.isOn)
            {
                AnimOnComplete();
            }
            else
            {
                AnimOffComplete();
            }
        }

        public override void Disable()
        {
            base.Disable();

            checkImage.color = disabledColor;
            frameImage.color = disabledColor;
        }

        public override void AnimOn()
        {
            base.AnimOn();

            checkImage.color = Tween.QuintOut(m_CurrentColor, onColor, m_AnimDeltaTime, animationDuration);
            frameImage.color = Tween.QuintOut(m_CurrentFrameColor, onColor, m_AnimDeltaTime, animationDuration);

            float tempSize = Tween.QuintOut(m_CurrentCheckSize, 24, m_AnimDeltaTime, animationDuration);

            checkRectTransform.sizeDelta = new Vector2(tempSize, tempSize);
        }

        public override void AnimOnComplete()
        {
            base.AnimOnComplete();

            checkImage.color = onColor;
            frameImage.color = onColor;

            checkRectTransform.sizeDelta = new Vector2(24, 24);
        }

        public override void AnimOff()
        {
            base.AnimOff();

            checkImage.color = Tween.QuintOut(m_CurrentColor, offColor, m_AnimDeltaTime, animationDuration);
            frameImage.color = Tween.QuintOut(m_CurrentFrameColor, offColor, m_AnimDeltaTime, animationDuration);

            float tempSize = Tween.QuintOut(m_CurrentCheckSize, 0, m_AnimDeltaTime, animationDuration);

            checkRectTransform.sizeDelta = new Vector2(tempSize, tempSize);
        }

        public override void AnimOffComplete()
        {
            base.AnimOffComplete();

            checkImage.color = offColor;
            frameImage.color = offColor;

            checkRectTransform.sizeDelta = new Vector2(0, 0);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (m_Interactable)
            {
                m_CheckImage.color = toggle.isOn ? m_OnColor : m_OffColor;
                m_FrameImage.color = toggle.isOn ? m_OnColor : m_OffColor;
            }
            else
            {
                m_CheckImage.color = m_DisabledColor;
                m_FrameImage.color = m_DisabledColor;
            }
        }
#endif
    }
}
