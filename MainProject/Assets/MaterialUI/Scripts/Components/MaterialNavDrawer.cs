//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/Material Nav Drawer", 100)]
    public class MaterialNavDrawer : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Image m_BackgroundImage;
        public Image backgroundImage
        {
            get { return m_BackgroundImage; }
            set { m_BackgroundImage = value; }
        }

        [SerializeField]
        private Image m_ShadowImage;
        public Image shadowImage
        {
            get { return m_ShadowImage; }
            set { m_ShadowImage = value; }
        }

        [SerializeField]
        private GameObject m_PanelLayer;
        public GameObject panelLayer
        {
            get { return m_PanelLayer; }
            set { m_PanelLayer = value; }
        }

        [SerializeField]
        private bool m_DarkenBackground = true;
        public bool darkenBackground
        {
            get { return m_DarkenBackground; }
            set { m_DarkenBackground = value; }
        }

        [SerializeField]
        private bool m_TapBackgroundToClose = true;
        public bool tapBackgroundToClose
        {
            get { return m_TapBackgroundToClose; }
            set { m_TapBackgroundToClose = value; }
        }

        [SerializeField]
        private bool m_OpenOnStart;
        public bool openOnStart
        {
            get { return m_OpenOnStart; }
            set { m_OpenOnStart = value; }
        }

        [SerializeField]
        private float m_AnimationDuration = 0.5f;
        public float animationDuration
        {
            get { return m_AnimationDuration; }
            set { m_AnimationDuration = value; }
        }

        private MaterialUIScaler m_Scaler;

        public MaterialUIScaler scaler
        {
            get
            {
                if (m_Scaler == null)
                {
                    m_Scaler = MaterialUIScaler.GetParentScaler(transform);
                }
                return m_Scaler;
            }
        }

        private float m_MaxPosition;
        private float m_MinPosition;
        private RectTransform m_RectTransform;

        private GameObject m_BackgroundGameObject;
        private RectTransform m_BackgroundRectTransform;
        private CanvasGroup m_BackgroundCanvasGroup;
        private GameObject m_ShadowGameObject;
        private CanvasGroup m_ShadowCanvasGroup;

        private byte m_AnimState;
        private float m_AnimStartTime;
        private float m_AnimDeltaTime;

        private Vector2 m_CurrentPos;
        private float m_CurrentBackgroundAlpha;
        private float m_CurrentShadowAlpha;

        private Vector2 m_TempVector2;

        void Awake()
        {
            m_RectTransform = gameObject.GetComponent<RectTransform>();
            m_BackgroundRectTransform = m_BackgroundImage.GetComponent<RectTransform>();
            m_BackgroundCanvasGroup = m_BackgroundImage.GetComponent<CanvasGroup>();
            m_ShadowCanvasGroup = m_ShadowImage.GetComponent<CanvasGroup>();
        }

        void Start()
        {
            m_MaxPosition = m_RectTransform.rect.width / 2;
            m_MinPosition = -m_MaxPosition;

            m_BackgroundRectTransform.sizeDelta = new Vector2((Screen.width / scaler.scaleFactor) + 1f, m_BackgroundRectTransform.sizeDelta.y);

            m_BackgroundGameObject = m_BackgroundImage.gameObject;
            m_ShadowGameObject = m_ShadowImage.gameObject;

            if (m_OpenOnStart)
            {
                Open();
            }
            else
            {
                m_BackgroundGameObject.SetActive(false);
                m_ShadowGameObject.SetActive(false);
                m_PanelLayer.SetActive(false);
            }
        }

        public void BackgroundTap()
        {
            if (m_TapBackgroundToClose)
            {
                Close();
            }
        }

        public void Open()
        {
            m_BackgroundGameObject.SetActive(true);
            m_ShadowGameObject.SetActive(true);
            m_PanelLayer.SetActive(true);
            m_CurrentPos = m_RectTransform.anchoredPosition;
            m_CurrentBackgroundAlpha = m_BackgroundCanvasGroup.alpha;
            m_CurrentShadowAlpha = m_ShadowCanvasGroup.alpha;
            m_BackgroundCanvasGroup.blocksRaycasts = true;
            m_AnimStartTime = Time.realtimeSinceStartup;
            m_AnimState = 1;
        }

        public void Close()
        {
            m_CurrentPos = m_RectTransform.anchoredPosition;
            m_CurrentBackgroundAlpha = m_BackgroundCanvasGroup.alpha;
            m_CurrentShadowAlpha = m_ShadowCanvasGroup.alpha;
            m_BackgroundCanvasGroup.blocksRaycasts = false;
            m_AnimStartTime = Time.realtimeSinceStartup;
            m_AnimState = 2;
        }

        void Update()
        {
            if (m_AnimState == 1)
            {
                m_AnimDeltaTime = Time.realtimeSinceStartup - m_AnimStartTime;

                if (m_AnimDeltaTime <= m_AnimationDuration)
                {
                    m_RectTransform.anchoredPosition = Tween.QuintOut(m_CurrentPos, new Vector2(m_MaxPosition, m_RectTransform.anchoredPosition.y), m_AnimDeltaTime, m_AnimationDuration);

                    if (m_DarkenBackground)
                    {
                        m_BackgroundCanvasGroup.alpha = Tween.QuintOut(m_CurrentBackgroundAlpha, 1f, m_AnimDeltaTime, m_AnimationDuration);
                    }

                    m_ShadowCanvasGroup.alpha = Tween.QuintIn(m_CurrentShadowAlpha, 1f, m_AnimDeltaTime, m_AnimationDuration / 2f);
                }
                else
                {
                    m_RectTransform.anchoredPosition = new Vector2(m_MaxPosition, m_RectTransform.anchoredPosition.y);
                    if (m_DarkenBackground)
                    {
                        m_BackgroundCanvasGroup.alpha = 1f;
                    }
                    m_AnimState = 0;
                }
            }
            else if (m_AnimState == 2)
            {
                m_AnimDeltaTime = Time.realtimeSinceStartup - m_AnimStartTime;

                if (m_AnimDeltaTime <= m_AnimationDuration)
                {
                    m_RectTransform.anchoredPosition = Tween.QuintOut(m_CurrentPos, new Vector2(m_MinPosition, m_RectTransform.anchoredPosition.y), m_AnimDeltaTime, m_AnimationDuration);

                    if (m_DarkenBackground)
                    {
                        m_BackgroundCanvasGroup.alpha = Tween.QuintOut(m_CurrentBackgroundAlpha, 0f, m_AnimDeltaTime, m_AnimationDuration);
                    }

                    m_ShadowCanvasGroup.alpha = Tween.QuintIn(m_CurrentShadowAlpha, 0f, m_AnimDeltaTime, m_AnimationDuration);
                }
                else
                {
                    m_RectTransform.anchoredPosition = new Vector2(m_MinPosition, m_RectTransform.anchoredPosition.y);
                    if (m_DarkenBackground)
                    {
                        m_BackgroundCanvasGroup.alpha = 0f;
                    }

                    m_BackgroundGameObject.SetActive(false);
                    m_ShadowGameObject.SetActive(false);
                    m_PanelLayer.SetActive(false);

                    m_AnimState = 0;
                }
            }

            m_RectTransform.anchoredPosition = new Vector2(Mathf.Clamp(m_RectTransform.anchoredPosition.x, m_MinPosition, m_MaxPosition), m_RectTransform.anchoredPosition.y);
        }

        public void OnBeginDrag(PointerEventData data)
        {
            m_AnimState = 0;

            m_BackgroundGameObject.SetActive(true);
            m_ShadowGameObject.SetActive(true);
            m_PanelLayer.SetActive(true);
        }

        public void OnDrag(PointerEventData data)
        {
            m_TempVector2 = m_RectTransform.anchoredPosition;
            m_TempVector2.x += data.delta.x / scaler.scaleFactor;

            m_RectTransform.anchoredPosition = m_TempVector2;

            if (m_DarkenBackground)
            {
                m_BackgroundCanvasGroup.alpha = 1 - (m_MaxPosition - m_RectTransform.anchoredPosition.x) / (m_MaxPosition - m_MinPosition);
            }

            m_ShadowCanvasGroup.alpha = 1 - (m_MaxPosition - m_RectTransform.anchoredPosition.x) / ((m_MaxPosition - m_MinPosition) * 2);
        }

        public void OnEndDrag(PointerEventData data)
        {
            if (Mathf.Abs(data.delta.x) >= 0.5f)
            {
                if (data.delta.x > 0.5f)
                {
                    Open();
                }
                else
                {
                    Close();
                }
            }
            else
            {
                if ((m_RectTransform.anchoredPosition.x - m_MinPosition) >
                    (m_MaxPosition - m_RectTransform.anchoredPosition.x))
                {
                    Open();
                }
                else
                {
                    Close();
                }
            }
        }
    }
}