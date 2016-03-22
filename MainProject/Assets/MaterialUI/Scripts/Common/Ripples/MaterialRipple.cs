//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/Material Ripple", 50)]
    [ExecuteInEditMode]
    public class MaterialRipple : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IRippleCreator, ISelectHandler, IDeselectHandler, ISubmitHandler
    {
        public enum HighlightActive
        {
            Never,
            Hovered,
            Clicked
        }

        [SerializeField]
        private RippleData m_RippleData = new RippleData();
        public RippleData rippleData
        {
            get { return m_RippleData; }
            set { m_RippleData = value; }
        }

        public Color rippleColor
        {
            get { return m_RippleData.Color; }
            set
            {
                m_RippleData.Color = value;
                RefreshSettings();
            }
        }

        [SerializeField]
        private Graphic m_HighlightGraphic;
        public Graphic highlightGraphic
        {
            get
            {
                if (m_HighlightGraphic == null)
                {
                    if (m_RippleData.RippleParent != null)
                    {
                        m_HighlightGraphic = m_RippleData.RippleParent.GetComponent<Graphic>();
                    }
                }
                return m_HighlightGraphic;
            }
            set { m_HighlightGraphic = value; }
        }

        [SerializeField]
        private HighlightActive m_HighlightWhen = HighlightActive.Clicked;
        public HighlightActive highlightWhen
        {
            get { return m_HighlightWhen; }
            set { m_HighlightWhen = value; }
        }

        [SerializeField]
        private bool m_AutoHighlightColor = true;
        public bool autoHighlightColor
        {
            get { return m_AutoHighlightColor; }
            set
            {
                m_AutoHighlightColor = value;
                if (value)
                {
                    RefreshSettings();
                }
            }
        }

        [SerializeField]
        private Color m_HighlightColor = Color.black;
        public Color highlightColor
        {
            get { return m_HighlightColor; }
            set { m_HighlightColor = value; }
        }

        [SerializeField]
        private bool m_ToggleMask = true;
        public bool toggleMask
        {
            get { return m_ToggleMask; }
            set { m_ToggleMask = value; }
        }

        [SerializeField]
        private bool m_CheckForScroll;
        public bool checkForScroll
        {
            get { return m_CheckForScroll; }
            set { m_CheckForScroll = value; }
        }

        private int m_RippleCount;
        public int rippleCount
        {
            get { return m_RippleCount; }
        }

        private Mask m_Mask;
        public Mask mask
        {
            get
            {
                if (m_Mask == null)
                {
                    if (m_ToggleMask && highlightGraphic != null)
                    {
                        m_Mask = highlightGraphic.GetAddComponent<Mask>();
                    }
                }
                return m_Mask;
            }
        }

        [SerializeField]
        private float m_AutoHighlightBlendAmount = 0.2f;
        public float autoHighlightBlendAmount
        {
            get { return m_AutoHighlightBlendAmount; }
            set
            {
                m_AutoHighlightBlendAmount = Mathf.Clamp(value, 0f, 1f);

            }
        }

        private Ripple m_CurrentRipple;

        private Color m_NormalColor;
        private Color m_CurrentColor;

        private int m_AnimState;
        private float m_AnimStartTime;
        private float m_AnimDeltaTime;
        private float m_AnimDuration;

        private bool m_ImageIsTransparent;
        private bool m_Clicked;

        private void Start()
        {
            if (!m_RippleData.RippleParent)
            {
                Image[] possibleTargets = GetComponentsInChildren<Image>();

                for (int i = 0; i < possibleTargets.Length; i++)
                {
                    if (!possibleTargets[i].GetComponent<AnimatedShadow>())
                    {
                        m_RippleData.RippleParent = possibleTargets[i].transform;
                        m_HighlightGraphic = null;
                        break;
                    }
                }

                Graphic tempGraphic = GetComponentInChildren<Graphic>();

                if (tempGraphic != null)
                {
                    m_RippleData.RippleParent = tempGraphic.transform;
                }
            }

            if (Application.isPlaying)
            {
                RefreshSettings();
            }
        }

        private void Update()
        {
            if (!highlightGraphic)
            {
                m_AnimState = 0;
                return;
            }

            if (m_AnimState == 1)
            {
                m_AnimDeltaTime = Time.realtimeSinceStartup - m_AnimStartTime;

                if (m_AnimDeltaTime <= m_AnimDuration)
                {
                    highlightGraphic.color = Tween.CubeOut(m_CurrentColor, highlightColor, m_AnimDeltaTime, m_AnimDuration);
                    CheckTransparency();
                }
                else
                {
                    highlightGraphic.color = highlightColor;
                    CheckTransparency();

                    m_AnimState = 0;
                }
            }
            else if (m_AnimState == 2)
            {
                m_AnimDeltaTime = Time.realtimeSinceStartup - m_AnimStartTime;

                if (m_AnimDeltaTime <= m_AnimDuration)
                {
                    highlightGraphic.color = Tween.CubeOut(m_CurrentColor, m_NormalColor, m_AnimDeltaTime, m_AnimDuration);
                    CheckTransparency();
                }
                else
                {
                    highlightGraphic.color = m_NormalColor;
                    CheckTransparency();

                    m_AnimState = 0;
                }
            }
        }

        private void CheckTransparency()
        {
            if (m_ImageIsTransparent)
            {
                if (highlightGraphic.color.a <= 0.015f)
                {
                    if (m_RippleCount > 0)
                    {
                        highlightGraphic.color = new Color(highlightGraphic.color.r, highlightGraphic.color.g,
                            highlightGraphic.color.b, 0.015f);
                    }
                    else
                    {
                        highlightGraphic.color = new Color(highlightGraphic.color.r, highlightGraphic.color.g,
                            highlightGraphic.color.b, 0);
                    }

                    if (m_Mask != null)
                    {
                        mask.showMaskGraphic = false;
                    }
                }
                else if (m_Mask != null)
                {
                    m_Mask.showMaskGraphic = true;
                }
            }
        }

        public void SetGraphicColor(Color color, bool animate = true)
        {
            if (animate)
            {
                m_NormalColor = color;

                if (m_AutoHighlightColor)
                {
                    m_HighlightColor = MaterialColor.GetHighlightColor(m_NormalColor, rippleData.Color, m_AutoHighlightBlendAmount);
                }

                m_CurrentColor = highlightGraphic.color;
                m_AnimStartTime = Time.realtimeSinceStartup;

                if (m_AnimState == 0)
                {
                    m_AnimState = 2;
                }
            }
            else
            {
                m_NormalColor = color;

                if (m_AutoHighlightColor)
                {
                    m_HighlightColor = MaterialColor.GetHighlightColor(m_NormalColor, rippleData.Color, m_AutoHighlightBlendAmount);
                }

                highlightGraphic.color = m_AnimState == 1 ? m_HighlightColor : m_NormalColor;
                m_AnimState = 0;
            }
        }

        public void RefreshSettings()
        {
            m_AnimDuration = 4f / rippleData.Speed;

            if (highlightGraphic)
            {
                m_ImageIsTransparent = (highlightGraphic.color.a == 0f && toggleMask);

                if (m_ToggleMask)
                {
                    if (highlightGraphic.GetComponent<Mask>())
                    {
                        Destroy(highlightGraphic.GetComponent<Mask>());
                    }
                }
            }

            if (!highlightGraphic)
            {
                return;
            }

            m_NormalColor = highlightGraphic.color;

            if (autoHighlightColor)
            {
                if (highlightWhen != HighlightActive.Never)
                {
                    highlightColor = MaterialColor.GetHighlightColor(m_NormalColor, rippleData.Color, m_AutoHighlightBlendAmount);

                    if (m_ImageIsTransparent)
                    {
                        m_NormalColor = new Color(highlightColor.r, highlightColor.g, highlightColor.b, 0f);
                        highlightGraphic.color = m_NormalColor;
                    }
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!enabled) return;

            if (highlightWhen == HighlightActive.Hovered)
            {
                Highlight(1);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_Clicked = true;

            DestroyRipple();

            if (!enabled) return;

            DestroyRipple();

            if (checkForScroll)
            {
                StartCoroutine(ScrollCheck());
            }
            else
            {
                CreateRipple(eventData.position);

                if (highlightWhen == HighlightActive.Clicked)
                {
                    Highlight(1);
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            DestroyRipple();
            m_Clicked = false;

            if (highlightWhen == HighlightActive.Clicked)
            {
                Highlight(2);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DestroyRipple();
            m_Clicked = false;

            if (highlightWhen != HighlightActive.Never)
            {
                Highlight(2);
            }
        }

        private void Highlight(int state)
        {
            if (!highlightGraphic)
            {
                return;
            }

            m_CurrentColor = highlightGraphic.color;
            m_AnimStartTime = Time.realtimeSinceStartup;
            m_AnimState = state;
        }

        private void CreateRipple(Vector2 position, bool oscillate = false)
        {
            m_CurrentRipple = RippleManager.instance.GetRipple();
            m_CurrentRipple.Setup(rippleData, position, this, oscillate);
            m_CurrentRipple.Expand();
        }

        private void DestroyRipple()
        {
            if (m_CurrentRipple)
            {
                m_CurrentRipple.Contract();
                m_CurrentRipple = null;
            }
        }

        private void OnFirstRippleCreate()
        {
            if (m_ImageIsTransparent && highlightGraphic.color.a < 0.015f)
            {
                highlightGraphic.color = new Color(highlightGraphic.color.r, highlightGraphic.color.g,
                        highlightGraphic.color.b, 0.015f);
            }
            if (toggleMask)
            {
                mask.enabled = true;
            }
        }

        private void OnLastRippleDestroy()
        {
            if (toggleMask)
            {
                if (m_ImageIsTransparent && highlightGraphic.color.a <= 0.015f)
                {
                    highlightGraphic.color = new Color(highlightGraphic.color.r, highlightGraphic.color.g,
                        highlightGraphic.color.b, 0f);
                }

                Destroy(m_Mask);
            }
        }

        public void OnCreateRipple()
        {
            if (rippleCount == 0)
            {
                OnFirstRippleCreate();
            }

            m_RippleCount++;
        }

        public void OnDestroyRipple()
        {
            m_RippleCount--;

            if (rippleCount == 0)
            {
                OnLastRippleDestroy();
            }
        }

        private IEnumerator ScrollCheck()
        {
            Vector2 startPos = Input.mousePosition;

            yield return new WaitForSeconds(0.04f);

            Vector2 endPos = Input.mousePosition;

            if (Vector2.Distance(startPos, endPos) < 2f)
            {
                CreateRipple(startPos);

                if (highlightWhen == HighlightActive.Clicked)
                {
                    Highlight(1);
                }
            }
        }

        private IEnumerator SelectCheck()
        {
            yield return new WaitForEndOfFrame();
            if (m_Clicked == false)
            {
                CreateRipple(m_RippleData.RippleParent.position, true);
                if (highlightWhen == HighlightActive.Hovered)
                {
                    Highlight(1);
                }
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            StartCoroutine(SelectCheck());
        }

        public void OnDeselect(BaseEventData eventData)
        {
            DestroyRipple();
            if (highlightWhen == HighlightActive.Hovered)
            {
                Highlight(2);
            }
            m_Clicked = false;
        }

        public void OnSubmit(BaseEventData eventData)
        {
            DestroyRipple();
            if (highlightWhen == HighlightActive.Hovered)
            {
                Highlight(2);
            }
            StartCoroutine(SelectCheck());
        }
    }
}