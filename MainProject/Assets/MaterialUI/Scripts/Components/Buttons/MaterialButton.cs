//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MaterialUI
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [ExecuteInEditMode]
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(CanvasGroup))]
    [AddComponentMenu("MaterialUI/Material Button", 100)]
    public class MaterialButton : UIBehaviour, ILayoutGroup, ILayoutElement, ILayoutSelfController
    {
        #region Variables

        private const string pathToCirclePrefab = "Assets/MaterialUI/Prefabs/Components/Buttons/Floating Action Button.prefab";
        private const string pathToRectPrefab = "Assets/MaterialUI/Prefabs/Components/Buttons/Button.prefab";

        [SerializeField]
        private RectTransform m_RectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (m_RectTransform == null)
                {
                    m_RectTransform = (RectTransform)transform;
                }
                return m_RectTransform;
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
                SetLayoutDirty();
            }
        }

        [SerializeField]
        private Button m_ButtonObject;
        public Button buttonObject
        {
            get
            {
                if (m_ButtonObject == null)
                {
                    m_ButtonObject = gameObject.GetAddComponent<Button>();
                }
                return m_ButtonObject;
            }
        }

        [SerializeField]
        private Graphic m_BackgroundImage;
        public Graphic backgroundImage
        {
            get { return m_BackgroundImage; }
            set { m_BackgroundImage = value; }
        }

        [SerializeField]
        private Text m_Text;
        public Text text
        {
            get { return m_Text; }
            set
            {
                m_Text = value;
                SetLayoutDirty();
            }
        }

        [SerializeField]
        private Graphic m_Icon;
        public Graphic icon
        {
            get { return m_Icon; }
            set
            {
                m_Icon = value;
                SetLayoutDirty();
            }
        }

        [SerializeField]
        private MaterialRipple m_MaterialRipple;
        public MaterialRipple materialRipple
        {
            get
            {
                if (m_MaterialRipple == null)
                {
                    m_MaterialRipple = GetComponent<MaterialRipple>();
                }
                return m_MaterialRipple;
            }
        }

        [SerializeField]
        private MaterialShadow m_MaterialShadow;
        public MaterialShadow materialShadow
        {
            get
            {
                if (m_MaterialShadow == null)
                {
                    m_MaterialShadow = GetComponent<MaterialShadow>();
                }
                return m_MaterialShadow;
            }
        }

        [SerializeField]
        private CanvasGroup m_CanvasGroup;
        public CanvasGroup canvasGroup
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
        private CanvasGroup m_ShadowsCanvasGroup;
        public CanvasGroup shadowsCanvasGroup
        {
            get { return m_ShadowsCanvasGroup; }
            set { m_ShadowsCanvasGroup = value; }
        }

        [SerializeField]
		private bool m_Interactable = true;
        public bool interactable
        {
            get { return m_Interactable; }
            set
            {
                m_Interactable = value;
                m_ButtonObject.interactable = m_Interactable;
                canvasGroup.alpha = m_Interactable ? 1f : 0.5f;
                canvasGroup.blocksRaycasts = m_Interactable;
                if (shadowsCanvasGroup)
                {
                    shadowsCanvasGroup.alpha = m_Interactable ? 1f : 0f;
                }
            }
        }

        [SerializeField]
        private Vector2 m_ContentPadding = new Vector2(30f, 18f);
        public Vector2 contentPadding
        {
            get { return m_ContentPadding; }
            set
            {
                m_ContentPadding = value;
                SetLayoutDirty();
            }
        }

        [SerializeField]
        private Vector2 m_ContentSize;
        public Vector2 contentSize
        {
            get { return m_ContentSize; }
        }

        [SerializeField]
        private Vector2 m_Size;
        public Vector2 size
        {
            get { return m_Size; }
        }

        [SerializeField]
        private bool m_FitWidthToContent;
        public bool fitWidthToContent
        {
            get { return m_FitWidthToContent; }
            set
            {
                m_FitWidthToContent = value;
                m_Tracker.Clear();
                SetLayoutDirty();
            }
        }

        [SerializeField]
        private bool m_FitHeightToContent;
        public bool fitHeightToContent
        {
            get { return m_FitHeightToContent; }
            set
            {
                m_FitHeightToContent = value;
                m_Tracker.Clear();
                SetLayoutDirty();
            }
        }

        [SerializeField]
        private bool m_ShadowFitsLayout;
        public bool shadowFitsLayout
        {
            get { return m_ShadowFitsLayout; }
            set
            {
                m_ShadowFitsLayout = value;
                SetLayoutDirty();
            }
        }

        [SerializeField]
        private bool m_IsCircularButton;
        public bool isCircularButton
        {
            get { return m_IsCircularButton; }
            set { m_IsCircularButton = value; }
        }

        [SerializeField]
        private bool m_IsRaisedButton;
        public bool isRaisedButton
        {
            get { return m_IsRaisedButton; }
            set { m_IsRaisedButton = value; }
        }

#if UNITY_EDITOR
        private Vector2 m_LastPosition;
#endif

        private DrivenRectTransformTracker m_Tracker = new DrivenRectTransformTracker();

        #endregion

#if UNITY_EDITOR
        public MaterialButton()
        {
            EditorUpdate.Init();
            EditorUpdate.onEditorUpdate += OnEditorUpdate;
        }
#endif

        protected override void OnEnable()
        {
            SetLayoutDirty();

#if UNITY_EDITOR
            OnValidate();
#endif
        }

        protected override void OnDisable()
        {
            m_Tracker.Clear();
        }

#if UNITY_EDITOR
        protected override void OnDestroy()
        {
            EditorUpdate.onEditorUpdate -= OnEditorUpdate;
        }
#endif

#if UNITY_EDITOR
        private void OnEditorUpdate()
        {
            if (IsDestroyed())
            {
                EditorUpdate.onEditorUpdate -= OnEditorUpdate;
                return;
            }

            if (rectTransform.anchoredPosition != m_LastPosition)
            {
                m_LastPosition = rectTransform.anchoredPosition;
                EditorUtility.SetDirty(rectTransform);
            }
        }
#endif

        public void Convert(bool noExitGUI = false)
        {
#if UNITY_EDITOR
            string flatRoundedSquare = "Assets/MaterialUI/Images/RoundedSquare/roundedsquare_";
            string raisedRoundedSquare = "Assets/MaterialUI/Images/RoundedSquare_Stroke/roundedsquare_stroke_";

            string imagePath = "";

            if (!isCircularButton)
            {
                imagePath = isRaisedButton ? flatRoundedSquare : raisedRoundedSquare;
            }

            if (isRaisedButton)
            {
                DestroyImmediate(m_ShadowsCanvasGroup.gameObject);
                m_ShadowsCanvasGroup = null;

                if (materialShadow)
                {
                    DestroyImmediate(materialShadow);
                }

                if (materialRipple != null)
                {
                    materialRipple.highlightWhen = MaterialRipple.HighlightActive.Hovered;
                }
            }
            else
            {
                string path = isCircularButton ? pathToCirclePrefab : pathToRectPrefab;

                GameObject tempButton = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(path));

                GameObject newShadow = tempButton.transform.FindChild("Shadows").gameObject;

                m_ShadowsCanvasGroup = newShadow.GetComponent<CanvasGroup>();

                RectTransform newShadowRectTransform = (RectTransform)newShadow.transform;

                newShadowRectTransform.SetParent(rectTransform);
                newShadowRectTransform.SetAsFirstSibling();
                newShadowRectTransform.localScale = Vector3.one;
                newShadowRectTransform.localEulerAngles = Vector3.zero;

                RectTransform tempRectTransform = m_BackgroundImage != null
                    ? (RectTransform)m_BackgroundImage.transform
                    : rectTransform;

                if (isCircularButton)
                {
                    newShadowRectTransform.anchoredPosition = Vector2.zero;
                    RectTransformSnap newSnapper = newShadow.GetAddComponent<RectTransformSnap>();
                    newSnapper.sourceRectTransform = tempRectTransform;
                    newSnapper.valuesArePercentage = true;
                    newSnapper.snapWidth = true;
                    newSnapper.snapHeight = true;
                    newSnapper.snapEveryFrame = true;
                    newSnapper.paddingPercent = new Vector2(225, 225);
                    Vector3 tempVector3 = rectTransform.GetPositionRegardlessOfPivot();
                    tempVector3.y -= 1f;
                    newShadowRectTransform.position = tempVector3;
                }
                else
                {
                    newShadowRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tempRectTransform.GetProperSize().x + 54);
                    newShadowRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tempRectTransform.GetProperSize().y + 54);
                    Vector3 tempVector3 = rectTransform.GetPositionRegardlessOfPivot();
                    newShadowRectTransform.position = tempVector3;
                }

                DestroyImmediate(tempButton);

                gameObject.AddComponent<MaterialShadow>();

                materialShadow.shadowsActiveWhen = MaterialShadow.ShadowsActive.Hovered;

                materialShadow.animatedShadows = newShadow.GetComponentsInChildren<AnimatedShadow>();

                materialShadow.isEnabled = true;

                if (materialRipple != null)
                {
                    materialRipple.highlightWhen = MaterialRipple.HighlightActive.Clicked;
                }
            }

            if (!isCircularButton)
            {
                SpriteSwapper spriteSwapper = GetComponent<SpriteSwapper>();

                if (spriteSwapper != null)
                {
                    spriteSwapper.sprite1X = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath + "100%.png");
                    spriteSwapper.sprite2X = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath + "200%.png");
                    spriteSwapper.sprite4X = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath + "400%.png");
                }
                else
                {
                    if (m_BackgroundImage != null)
                    {
                        ((Image)m_BackgroundImage).sprite = AssetDatabase.LoadAssetAtPath<Sprite>(imagePath + "100%.png");
                    }
                }
            }
            else
            {
                if (!isRaisedButton)
                {

                    RectTransform tempRectTransform = (RectTransform)new GameObject("Stroke", typeof(VectorImage)).transform;

                    tempRectTransform.SetParent(m_BackgroundImage.rectTransform);
                    tempRectTransform.localScale = Vector3.one;
                    tempRectTransform.localEulerAngles = Vector3.zero;
                    tempRectTransform.anchorMin = Vector2.zero;
                    tempRectTransform.anchorMax = Vector2.one;
                    tempRectTransform.anchoredPosition = Vector2.zero;
                    tempRectTransform.sizeDelta = Vector2.zero;

                    VectorImage vectorImage = tempRectTransform.GetComponent<VectorImage>();
                    vectorImage.vectorImageData = MaterialUIIconHelper.GetIcon("circle_stroke_thin").vectorImageData;
                    vectorImage.sizeMode = VectorImage.SizeMode.MatchMin;
                    vectorImage.color = new Color(0f, 0f, 0f, 0.125f);

                    tempRectTransform.name = "Stroke";
                }
                else
                {
                    VectorImage[] images = backgroundImage.GetComponentsInChildren<VectorImage>();

                    for (int i = 0; i < images.Length; i++)
                    {
                        if (images[i].name == "Stroke")
                        {
                            DestroyImmediate(images[i].gameObject);
                        }
                    }
                }
            }

            name = isRaisedButton ? name.Replace("Raised", "Flat") : name.Replace("Flat", "Raised");

            if (m_BackgroundImage != null)
            {
                if (!isRaisedButton)
                {
                    if (m_BackgroundImage.color == Color.clear)
                    {
                        m_BackgroundImage.color = Color.white;
                    }
                }
                else
                {

                    if (m_BackgroundImage.color == Color.white)
                    {
                        m_BackgroundImage.color = Color.clear;
                    }
                }
            }

            m_IsRaisedButton = !m_IsRaisedButton;

            if (!noExitGUI)
            {
                GUIUtility.ExitGUI();
            }
#endif
        }

        public void ClearTracker()
        {
            m_Tracker.Clear();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            SetLayoutDirty();
        }

        protected override void OnCanvasGroupChanged()
        {
            SetLayoutDirty();
        }

        protected override void OnDidApplyAnimationProperties()
        {
            SetLayoutDirty();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            if (m_RectTransform == null)
            {
                m_RectTransform = GetComponent<RectTransform>();
            }
            if (m_ButtonObject == null)
            {
                m_ButtonObject = gameObject.GetAddComponent<Button>();
            }
            if (m_CanvasGroup == null)
            {
                m_CanvasGroup = gameObject.GetAddComponent<CanvasGroup>();
            }

            SetLayoutDirty();
        }
#endif

        public void SetLayoutDirty()
        {
            LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
        }

        public void SetLayoutHorizontal()
        {
            if (m_FitWidthToContent)
            {
                if (m_ContentRectTransform == null) return;
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaX);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_Size.x);
                m_Tracker.Add(this, m_ContentRectTransform, DrivenTransformProperties.SizeDeltaX);
                m_ContentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_ContentSize.x);
            }
        }

        public void SetLayoutVertical()
        {
            if (m_FitHeightToContent)
            {
                if (m_ContentRectTransform == null) return;
                m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDeltaY);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_Size.y);
                m_Tracker.Add(this, m_ContentRectTransform, DrivenTransformProperties.SizeDeltaY);
                m_ContentRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_ContentSize.y);
            }
        }

        public void CalculateLayoutInputHorizontal()
        {
            if (m_FitWidthToContent)
            {
                if (m_ContentRectTransform == null) return;
                m_ContentSize.x = LayoutUtility.GetPreferredWidth(m_ContentRectTransform);
                m_Size.x = m_ContentSize.x + m_ContentPadding.x;
            }
            else
            {
                m_Size.x = -1;
            }
        }

        public void CalculateLayoutInputVertical()
        {
            if (m_FitHeightToContent)
            {
                if (m_ContentRectTransform == null) return;
                m_ContentSize.y = LayoutUtility.GetPreferredHeight(m_ContentRectTransform);
                m_Size.y = m_ContentSize.y + m_ContentPadding.y;
            }
            else
            {
                m_Size.y = -1;
            }
        }

        public float minWidth { get { return enabled ? m_Size.x : 0; } }
        public float preferredWidth { get { return minWidth; } }
        public float flexibleWidth { get { return -1; } }
        public float minHeight { get { return enabled ? m_Size.y : 0; } }
        public float preferredHeight { get { return minHeight; } }
        public float flexibleHeight { get { return -1; } }
        public int layoutPriority { get { return 1; } }
    }
}