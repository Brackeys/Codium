//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MaterialUI
{
    public class ToggleBase : UIBehaviour
    {
        [SerializeField]
        protected float m_AnimationDuration = 0.5f;
        public float animationDuration
        {
            get { return m_AnimationDuration; }
            set { m_AnimationDuration = value; }
        }

        [SerializeField]
        protected Color m_OnColor = Color.black;
        public Color onColor
        {
            get { return m_OnColor; }
            set { m_OnColor = value; }
        }

        [SerializeField]
        protected Color m_OffColor = Color.black;
        public Color offColor
        {
            get { return m_OffColor; }
            set { m_OffColor = value; }
        }

        [SerializeField]
        protected Color m_DisabledColor = Color.black;
        public Color disabledColor
        {
            get { return m_DisabledColor; }
            set { m_DisabledColor = value; }
        }

        [SerializeField]
        protected bool m_ChangeGraphicColor = true;
        public bool changeGraphicColor
        {
            get { return m_ChangeGraphicColor; }
            set { m_ChangeGraphicColor = value; }
        }

        [SerializeField]
        protected Color m_GraphicOnColor = Color.black;
        public Color graphicOnColor
        {
            get { return m_GraphicOnColor; }
            set { m_GraphicOnColor = value; }
        }

        [SerializeField]
        protected Color m_GraphicOffColor = Color.black;
        public Color graphicOffColor
        {
            get { return m_GraphicOffColor; }
            set { m_GraphicOffColor = value; }
        }

        [SerializeField]
        protected Color m_GraphicDisabledColor = Color.black;
        public Color graphicDisabledColor
        {
            get { return m_GraphicDisabledColor; }
            set { m_GraphicDisabledColor = value; }
        }

        [SerializeField]
        protected bool m_ChangeRippleColor = true;
        public bool changeRippleColor
        {
            get { return m_ChangeRippleColor; }
            set { m_ChangeRippleColor = value; }
        }

        [SerializeField]
        protected Color m_RippleOnColor = Color.black;
        public Color rippleOnColor
        {
            get { return m_RippleOnColor; }
            set { m_RippleOnColor = value; }
        }

        [SerializeField]
        protected Color m_RippleOffColor = Color.black;
        public Color rippleOffColor
        {
            get { return m_RippleOffColor; }
            set { m_RippleOffColor = value; }
        }

        [SerializeField]
        protected internal Graphic m_Graphic;
        public Graphic graphic
        {
            get { return m_Graphic; }
            set { m_Graphic = value; }
        }

        [SerializeField]
        protected bool m_ToggleGraphic;
        public bool toggleGraphic
        {
            get { return m_ToggleGraphic; }
            set { m_ToggleGraphic = value; }
        }

        [SerializeField]
        protected string m_ToggleOnLabel;
        public string toggleOnLabel
        {
            get { return m_ToggleOnLabel; }
            set { m_ToggleOnLabel = value; }
        }

        [SerializeField]
        protected string m_ToggleOffLabel;
        public string toggleOffLabel
        {
            get { return m_ToggleOffLabel; }
            set { m_ToggleOffLabel = value; }
        }

        [SerializeField]
        protected ImageData m_ToggleOnIcon;
        public ImageData toggleOnIcon
        {
            get { return m_ToggleOnIcon; }
            set { m_ToggleOnIcon = value; }
        }

        [SerializeField]
        protected ImageData m_ToggleOffIcon;
        public ImageData toggleOffIcon
        {
            get { return m_ToggleOffIcon; }
            set { m_ToggleOffIcon = value; }
        }

        protected MaterialRipple m_MaterialRipple;
        public MaterialRipple materialRipple
        {
            get { return m_MaterialRipple; }
            set { m_MaterialRipple = value; }
        }

        protected Toggle m_Toggle;
        public Toggle toggle
        {
            get
            {
                if (!m_Toggle)
                {
                    m_Toggle = gameObject.GetComponent<Toggle>();
                }
                return m_Toggle;
            }
            set { m_Toggle = value; }
        }

        public string labelText
        {
            get
            {
                if (m_Graphic == null) return null;

                Text text = m_Graphic as Text;
                if (text != null)
                {
                    return text.text;
                }

                return null;
            }

            set
            {
                if (m_Graphic == null) return;

                Text text = m_Graphic as Text;
                if (text != null)
                {
                    text.text = value;
                }
            }
        }

        public ImageData icon
        {
            get
            {
                if (m_Graphic == null) return null;

                return m_Graphic.GetImageData();
            }

            set
            {
                if (m_Graphic == null) return;

                m_Graphic.SetImage(value);
            }
        }

        protected CanvasGroup m_CanvasGroup;
        private CanvasGroup canvasGroup
        {
            get
            {
                if (m_CanvasGroup == null)
                {
                    m_CanvasGroup = gameObject.GetAddComponent<CanvasGroup>();
                }

                return m_CanvasGroup;
            }
        }

        [SerializeField]
        protected bool m_Interactable = true;
        public bool interactable
        {
            get { return m_Interactable; }
            set
            {
                m_Interactable = value;
                toggle.interactable = value;

                if (value)
                {
                    Enable();
                }
                else
                {
                    Disable();
                }

                UpdateGraphicToggleState();
            }
        }

        [SerializeField]
        protected ImageData m_Icon;

        [SerializeField]
        protected string m_Label;

        protected VectorImageData m_LastIconVectorImageData;
        protected Sprite m_LastIconSprite;

        protected string m_LastLabelText;

#if UNITY_EDITOR
        [SerializeField]
        protected bool m_LastToggleState;
#endif

        protected Color m_CurrentColor;
        protected Color m_CurrentGraphicColor;

        protected int m_AnimState;
        protected float m_AnimStartTime;
        protected float m_AnimDeltaTime;

        protected override void OnEnable()
        {
            m_Toggle = gameObject.GetComponent<Toggle>();
            materialRipple = gameObject.GetComponent<MaterialRipple>();
        }

        protected override void Start()
        {
            base.Start();

            UpdateGraphicToggleState();
        }

        public void Toggle(bool state)
        {
            if (m_Toggle.isOn)
            {
                TurnOn();
            }
            else
            {
                TurnOff();
            }
        }

        protected void UpdateGraphicToggleState()
        {
            UpdateIconDataType();

            if (m_Graphic == null || m_Toggle == null || !m_ToggleGraphic) return;

            Type graphicType = m_Graphic.GetType();

            if (graphicType == typeof(Image) || graphicType == typeof(VectorImage))
            {
                m_Graphic.SetImage(m_Toggle.isOn ? m_ToggleOnIcon : m_ToggleOffIcon);
            }
            else if (graphicType == typeof(Text))
            {
                ((Text)m_Graphic).text = m_Toggle.isOn ? m_ToggleOnLabel : m_ToggleOffLabel;
            }
        }

        protected void UpdateIconDataType()
        {
            if (m_Graphic == null) return;

            Type graphicType = m_Graphic.GetType();

            if (graphicType == typeof(Image))
            {
                m_ToggleOnIcon.imageDataType = ImageDataType.Sprite;
                m_ToggleOffIcon.imageDataType = ImageDataType.Sprite;
                m_Icon.imageDataType = ImageDataType.Sprite;
            }
            else if (graphicType == typeof(VectorImage))
            {
                m_ToggleOnIcon.imageDataType = ImageDataType.VectorImage;
                m_ToggleOffIcon.imageDataType = ImageDataType.VectorImage;
                m_Icon.imageDataType = ImageDataType.VectorImage;
            }
        }

        public virtual void TurnOn()
        {
            if (m_Graphic)
            {
                m_CurrentGraphicColor = m_Graphic.color;
            }

            AnimOn();
            m_AnimStartTime = Time.realtimeSinceStartup;
            m_AnimState = 1;

            UpdateGraphicToggleState();
        }

        public virtual void TurnOnInstant()
        {
            if (m_Interactable)
            {
                SetOnColor();
            }

            UpdateGraphicToggleState();
        }

        public virtual void TurnOff()
        {
            if (m_Graphic)
            {
                m_CurrentGraphicColor = m_Graphic.color;
            }

            AnimOff();
            m_AnimStartTime = Time.realtimeSinceStartup;
            m_AnimState = 2;

            UpdateGraphicToggleState();
        }

        public virtual void TurnOffInstant()
        {
            if (m_Interactable)
            {
                SetOffColor();
            }

            UpdateGraphicToggleState();
        }

        public virtual void Enable()
        {
            if (m_Toggle.isOn)
            {
                SetOnColor();
            }
            else
            {
                SetOffColor();
            }

            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        public virtual void Disable()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;

#if UNITY_EDITOR
            OnValidate();
#endif
        }

        void Update()
        {
            m_AnimDeltaTime = Time.realtimeSinceStartup - m_AnimStartTime;

            if (m_AnimState == 1)
            {
                if (m_AnimDeltaTime <= animationDuration)
                {
                    AnimOn();
                }
                else
                {
                    AnimOnComplete();
                }
            }
            else if (m_AnimState == 2)
            {
                if (m_AnimDeltaTime <= animationDuration)
                {
                    AnimOff();
                }
                else
                {
                    AnimOffComplete();
                }
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (m_LastToggleState != m_Toggle.isOn)
                {
                    m_LastToggleState = m_Toggle.isOn;

                    if (m_LastToggleState)
                    {
                        TurnOnInstant();
                    }
                    else
                    {
                        TurnOffInstant();
                    }
                }
            }
#endif
        }

        public virtual void AnimOn()
        {
            if (m_Graphic && changeGraphicColor)
            {
                m_Graphic.color = Tween.QuintSoftOut(m_CurrentGraphicColor, m_Interactable ? m_GraphicOnColor : m_GraphicDisabledColor, m_AnimDeltaTime, animationDuration);
            }
            if (m_ChangeRippleColor && m_MaterialRipple != null)
            {
                materialRipple.rippleData.Color = m_RippleOnColor;
            }
        }

        public virtual void AnimOnComplete()
        {
            SetOnColor();

            m_AnimState = 0;
        }

        public virtual void AnimOff()
        {
            if (m_Graphic && m_ChangeGraphicColor)
            {
                m_Graphic.color = Tween.QuintSoftOut(m_CurrentGraphicColor, m_Interactable ? m_GraphicOffColor : m_GraphicDisabledColor, m_AnimDeltaTime, animationDuration * 0.75f);
            }
            if (m_ChangeRippleColor && m_MaterialRipple != null)
            {
                materialRipple.rippleData.Color = m_RippleOffColor;
            }
        }

        public virtual void AnimOffComplete()
        {
            SetOffColor();

            m_AnimState = 0;
        }

        private void SetOnColor()
        {
            if (m_Graphic && m_ChangeGraphicColor)
            {
                m_Graphic.color = m_Interactable ? m_GraphicOnColor : m_GraphicDisabledColor;
            }
            if (materialRipple && m_ChangeRippleColor)
            {
                materialRipple.rippleData.Color = m_RippleOnColor;
            }
        }

        private void SetOffColor()
        {
            if (m_Graphic && m_ChangeGraphicColor)
            {
                m_Graphic.color = m_Interactable ? m_GraphicOffColor : m_GraphicDisabledColor;
            }
            if (materialRipple && m_ChangeRippleColor)
            {
                materialRipple.rippleData.Color = m_RippleOffColor;
            }
        }

#if UNITY_EDITOR

        public void EditorValidate()
        {
            OnValidate();
        }

        protected override void OnValidate()
        {
            if (m_Graphic && m_ChangeGraphicColor)
            {
                if (m_Interactable)
                {
                    m_Graphic.color = toggle.isOn ? m_GraphicOnColor : m_GraphicOffColor;
                }
                else
                {
                    m_Graphic.color = m_GraphicDisabledColor;
                }
            }
            if (materialRipple && m_ChangeRippleColor)
            {
                materialRipple.rippleData.Color = toggle.isOn ? m_RippleOnColor : m_RippleOffColor;
            }

            UpdateGraphicToggleState();

            if (m_Graphic != null)
            {
                if (m_Graphic.IsSpriteOrVectorImage())
                {
                    if (m_Graphic.GetType() == typeof(Image))
                    {
                        if (m_LastIconSprite == m_Icon.sprite)
                        {
                            m_Icon.sprite = m_Graphic.GetSpriteImage();
                        }
                        else
                        {
                            m_Graphic.SetImage(m_Icon.sprite);
                        }

                        m_LastIconSprite = m_Icon.sprite;
                    }
                    else
                    {
                        if (m_LastIconVectorImageData == m_Icon.vectorImageData)
                        {
                            m_Icon.vectorImageData = m_Graphic.GetVectorImage();
                        }
                        else
                        {
                            m_Graphic.SetImage(m_Icon.vectorImageData);
                        }

                        m_LastIconVectorImageData = m_Icon.vectorImageData;
                    }
                }
                else
                {
                    Text text = m_Graphic as Text;
                    if (text != null)
                    {
                        if (m_LastLabelText == m_Label)
                        {
                            m_Label = text.text;
                        }
                        else
                        {
                            text.text = m_Label;
                        }

                        m_LastLabelText = m_Label;
                    }
                }
            }

        }
#endif
    }
}