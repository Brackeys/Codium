//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    [ExecuteInEditMode]
    [AddComponentMenu("MaterialUI/Material Switch", 100)]
    public class MaterialSwitch : ToggleBase
    {
        [SerializeField]
        private Color m_BackOnColor = Color.black;
        public Color backOnColor
        {
            get { return m_BackOnColor; }
            set { m_BackOnColor = value; }
        }

        [SerializeField]
        private Color m_BackOffColor = Color.black;
        public Color backOffColor
        {
            get { return m_BackOffColor; }
            set { m_BackOffColor = value; }
        }

        [SerializeField]
        private Color m_BackDisabledColor = Color.black;
        public Color backDisabledColor
        {
            get { return m_BackDisabledColor; }
            set { m_BackDisabledColor = value; }
        }

        [SerializeField]
        private Graphic m_SwitchImage;
        public Graphic switchImage
        {
            get { return m_SwitchImage; }
            set { m_SwitchImage = value; }
        }

        [SerializeField]
        private Graphic m_BackImage;
        public Graphic backImage
        {
            get { return m_BackImage; }
            set { m_BackImage = value; }
        }

        [SerializeField]
        private Graphic m_ShadowImage;
        public Graphic shadowImage
        {
            get { return m_ShadowImage; }
            set { m_ShadowImage = value; }
        }

        private RectTransform m_SwitchRectTransform;
        public RectTransform switchRectTransform
        {
            get
            {
                if (m_SwitchRectTransform == null)
                {
                    if (m_SwitchImage != null)
                    {
                        m_SwitchRectTransform = (RectTransform)m_SwitchImage.transform;
                    }
                }
                return m_SwitchRectTransform;
            }
        }

        private float m_CurrentSwitchPosition;
        private Color m_CurrentBackColor;

        public override void TurnOn()
        {
            m_CurrentSwitchPosition = switchRectTransform.anchoredPosition.x;
            m_CurrentColor = switchImage.color;
            m_CurrentBackColor = backImage.color;

            base.TurnOn();
        }

        public override void TurnOnInstant()
        {
            base.TurnOnInstant();

            if (toggle.interactable)
            {
                switchImage.color = m_OnColor;
                backImage.color = backOnColor;
            }

            switchRectTransform.anchoredPosition = new Vector2(8f, 0f);
        }

        public override void TurnOff()
        {
            m_CurrentSwitchPosition = switchRectTransform.anchoredPosition.x;
            m_CurrentColor = switchImage.color;
            m_CurrentBackColor = backImage.color;

            base.TurnOff();
        }

        public override void TurnOffInstant()
        {
            base.TurnOffInstant();

            if (toggle.interactable)
            {
                switchImage.color = m_OffColor;
                backImage.color = backOffColor;
            }

            switchRectTransform.anchoredPosition = new Vector2(-8f, 0f);
        }

        public override void Enable()
        {
            if (toggle.isOn)
            {
                switchImage.color = m_OnColor;
                backImage.color = backOnColor;
            }
            else
            {
                switchImage.color = m_OffColor;
                backImage.color = backOffColor;
            }

            shadowImage.enabled = true;

            base.Enable();
        }

        public override void Disable()
        {
            switchImage.color = m_DisabledColor;
            backImage.color = backDisabledColor;

            shadowImage.enabled = false;

            base.Disable();
        }

        public override void AnimOn()
        {
            base.AnimOn();

            switchImage.color = Tween.QuintOut(m_CurrentColor, m_OnColor, m_AnimDeltaTime, m_AnimationDuration);
            backImage.color = Tween.QuintOut(m_CurrentBackColor, backOnColor, m_AnimDeltaTime, m_AnimationDuration);

            switchRectTransform.anchoredPosition = Tween.SeptSoftOut(new Vector2(m_CurrentSwitchPosition, 0f), new Vector2(8f, 0f), m_AnimDeltaTime, m_AnimationDuration);
        }

        public override void AnimOnComplete()
        {
            base.AnimOnComplete();

            switchImage.color = m_OnColor;
            backImage.color = backOnColor;

            switchRectTransform.anchoredPosition = new Vector2(8f, 0f);
        }

        public override void AnimOff()
        {
            base.AnimOff();

            switchImage.color = Tween.QuintOut(m_CurrentColor, m_OffColor, m_AnimDeltaTime, m_AnimationDuration);
            backImage.color = Tween.QuintOut(m_CurrentBackColor, backOffColor, m_AnimDeltaTime, m_AnimationDuration);

            switchRectTransform.anchoredPosition = Tween.SeptSoftOut(new Vector2(m_CurrentSwitchPosition, 0f), new Vector2(-8f, 0f), m_AnimDeltaTime, m_AnimationDuration);
        }

        public override void AnimOffComplete()
        {
            base.AnimOffComplete();

            switchImage.color = m_OffColor;
            backImage.color = backOffColor;

            switchRectTransform.anchoredPosition = new Vector2(-8f, 0f);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (m_Interactable)
            {
                m_SwitchImage.color = toggle.isOn ? m_OnColor : m_OffColor;
                m_BackImage.color = toggle.isOn ? m_BackOnColor : m_BackOffColor;
            }
            else
            {
                m_SwitchImage.color = m_DisabledColor;
                m_BackImage.color = m_BackDisabledColor;
            }
        }
#endif
    }
}
