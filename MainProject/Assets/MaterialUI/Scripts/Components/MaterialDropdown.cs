//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/Dropdown", 100)]
    public class MaterialDropdown : UIBehaviour, IOptionDataListContainer
    {
        public enum VerticalPivotType
        {
            BelowBase,
            Top,
            FirstItem,
            Center,
            LastItem,
            Bottom,
            AboveBase
        }

        public enum HorizontalPivotType
        {
            Left,
            Center,
            Right
        }

        public enum ExpandStartType
        {
            ExpandFromNothing,
            ExpandFromBaseTransformWidth,
            ExpandFromBaseTransformHeight,
            ExpandFromBaseTransformSize
        }

        [Serializable]
        public class DropdownListItem
        {
            public RectTransform m_RectTransform;
            public RectTransform rectTransform
            {
                get { return m_RectTransform; }
                set { m_RectTransform = value; }
            }

            public CanvasGroup m_CanvasGroup;
            public CanvasGroup canvasGroup
            {
                get { return m_CanvasGroup; }
                set { m_CanvasGroup = value; }
            }

            public Text m_Text;
            public Text text
            {
                get { return m_Text; }
                set { m_Text = value; }
            }

            public Graphic m_Image;
            public Graphic image
            {
                get { return m_Image; }
                set { m_Image = value; }
            }
        }

        [Serializable]
        public class MaterialDropdownEvent : UnityEvent<int> { }

        [SerializeField]
        private VerticalPivotType m_VerticalPivotType = VerticalPivotType.FirstItem;
        public VerticalPivotType verticalPivotType
        {
            get { return m_VerticalPivotType; }
            set { m_VerticalPivotType = value; }
        }

        [SerializeField]
        private HorizontalPivotType m_HorizontalPivotType = HorizontalPivotType.Left;
        public HorizontalPivotType horizontalPivotType
        {
            get { return m_HorizontalPivotType; }
            set { m_HorizontalPivotType = value; }
        }

        [SerializeField]
        private ExpandStartType m_ExpandStartType = ExpandStartType.ExpandFromBaseTransformSize;
        public ExpandStartType expandStartType
        {
            get { return m_ExpandStartType; }
            set { m_ExpandStartType = value; }
        }

        [SerializeField]
        private float m_IgnoreInputAfterShowTimer;
        public float ignoreInputAfterShowTimer
        {
            get { return m_IgnoreInputAfterShowTimer; }
            set { m_IgnoreInputAfterShowTimer = value; }
        }

        [SerializeField]
        private float m_MaxHeight = 200;
        public float maxHeight
        {
            get { return m_MaxHeight; }
            set { m_MaxHeight = value; }
        }

        [SerializeField]
        private bool m_CapitalizeButtonText = true;
        public bool capitalizeButtonText
        {
            get { return m_CapitalizeButtonText; }
            set { m_CapitalizeButtonText = value; }
        }

        [SerializeField]
        private bool m_HighlightCurrentlySelected = true;
        public bool highlightCurrentlySelected
        {
            get { return m_HighlightCurrentlySelected; }
            set { m_HighlightCurrentlySelected = value; }
        }

        [SerializeField]
        private bool m_UpdateHeader = true;
        public bool updateHeader
        {
            get { return m_UpdateHeader; }
            set { m_UpdateHeader = value; }
        }

        [SerializeField]
        private float m_AnimationDuration = 0.3f;
        public float animationDuration
        {
            get { return m_AnimationDuration; }
            set { m_AnimationDuration = value; }
        }

        [SerializeField]
        private float m_MinDistanceFromEdge = 16f;
        public float minDistanceFromEdge
        {
            get { return m_MinDistanceFromEdge; }
            set { m_MinDistanceFromEdge = value; }
        }

        [SerializeField]
        private Color m_PanelColor = Color.white;
        public Color panelColor
        {
            get { return m_PanelColor; }
            set { m_PanelColor = value; }
        }

        [SerializeField]
        private RippleData m_ItemRippleData;
        public RippleData itemRippleData
        {
            get { return m_ItemRippleData; }
            set { m_ItemRippleData = value; }
        }

        [SerializeField]
        private Color m_ItemTextColor = MaterialColor.textDark;
        public Color itemTextColor
        {
            get { return m_ItemTextColor; }
            set { m_ItemTextColor = value; }
        }

        [SerializeField]
        private Color m_ItemIconColor = MaterialColor.iconDark;
        public Color itemIconColor
        {
            get { return m_ItemIconColor; }
            set { m_ItemIconColor = value; }
        }

        [SerializeField]
        private RectTransform m_BaseTransform;
        public RectTransform baseTransform
        {
            get { return m_BaseTransform; }
            set { m_BaseTransform = value; }
        }

        [SerializeField]
        private Selectable m_BaseSelectable;
        public Selectable baseSelectable
        {
            get { return m_BaseSelectable; }
            set { m_BaseSelectable = value; }
        }

        [SerializeField]
        private Text m_ButtonTextContent;
        public Text buttonTextContent
        {
            get { return m_ButtonTextContent; }
            set { m_ButtonTextContent = value; }
        }

        [SerializeField]
        private Graphic m_ButtonImageContent;
        public Graphic buttonImageContent
        {
            get { return m_ButtonImageContent; }
            set { m_ButtonImageContent = value; }
        }

        [SerializeField]
        private int m_CurrentlySelected;
        public int currentlySelected
        {
            get { return m_CurrentlySelected; }
            set
            {
                m_CurrentlySelected = Mathf.Clamp(value, -1, m_OptionDataList.options.Count - 1);

                if (m_CurrentlySelected >= 0)
                {
                    if (m_ButtonImageContent != null)
                    {
                        m_ButtonImageContent.SetImage(m_OptionDataList.options[m_CurrentlySelected].imageData);
                    }

                    if (m_ButtonTextContent != null)
                    {
                        string itemText = m_OptionDataList.options[m_CurrentlySelected].text;

                        if (m_CapitalizeButtonText)
                        {
                            itemText = itemText.ToUpper();
                        }

                        m_ButtonTextContent.text = itemText;
                    }
                }
            }
        }

        [SerializeField]
        private OptionDataList m_OptionDataList;
        public OptionDataList optionDataList
        {
            get { return m_OptionDataList; }
            set { m_OptionDataList = value; }
        }

        [SerializeField]
        private MaterialDropdownEvent m_OnItemSelected = new MaterialDropdownEvent();
        public MaterialDropdownEvent onItemSelected
        {
            get { return m_OnItemSelected; }
            set { m_OnItemSelected = value; }
        }

        private List<DropdownListItem> m_ListItems = new List<DropdownListItem>();
        public List<DropdownListItem> listItems
        {
            get { return m_ListItems; }
            set { m_ListItems = value; }
        }

        private RectTransform m_DropdownPanel;
        private RectTransform m_PanelLayer;
        private CanvasGroup m_DropdownCanvasGroup;
        private Canvas m_DropdownCanvas;
        private CanvasGroup m_ShadowCanvasGroup;
        private DropdownListItem m_ListItemTemplate;
        private RectTransform m_CancelPanel;

        private Vector2 m_ExpandedSize;
        private Vector3 m_ExpandedPosition;
        private float m_FullHeight;
        private bool m_IsExapanded;
        private float m_TempMaxHeight;
        private float m_ScrollPosOffset;
        private float m_TimeShown;

        private GameObject m_DropdownCanvasGameObject;

        private List<int> m_AutoTweeners;
        private List<int> m_ListItemAutoTweeners = new List<int>();

        public void AddData(OptionData data)
        {
            m_OptionDataList.options.Add(data);
        }

        public void AddData(OptionData[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                AddData(data[i]);
            }
        }

        public void RemoveData(OptionData data)
        {
            m_OptionDataList.options.Remove(data);

            m_CurrentlySelected = Mathf.Clamp(m_CurrentlySelected, 0, m_OptionDataList.options.Count - 1);
        }

        public void RemoveData(OptionData[] data)
        {
            for (int i = 0; i < data.Length; i++)
            {
                RemoveData(data[i]);
            }
        }

        public void ClearData()
        {
            m_OptionDataList.options.Clear();

            m_CurrentlySelected = Mathf.Clamp(m_CurrentlySelected, 0, m_OptionDataList.options.Count - 1);
        }

        protected override void Start()
        {
            Canvas[] canvasses = FindObjectsOfType<Canvas>();

            for (int i = 0; i < canvasses.Length; i++)
            {
                if (canvasses[i].name == "Dropdown Canvas")
                {
                    m_DropdownCanvasGameObject = canvasses[i].gameObject;
                }
            }

            if (m_DropdownCanvasGameObject == null)
            {
                m_DropdownCanvasGameObject = new GameObject("Dropdown Canvas");
            }

            m_DropdownCanvas = m_BaseTransform.root.GetComponent<Canvas>().Copy(m_DropdownCanvasGameObject);
        }

        public void Show()
        {
            m_BaseTransform.root.GetComponent<Canvas>().CopySettingsToOtherCanvas(m_DropdownCanvas);
            m_DropdownCanvas.pixelPerfect = true;
            m_DropdownCanvas.sortingOrder = 30000;

            m_DropdownPanel = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.dropdownPanel, m_DropdownCanvas.transform).GetComponent<RectTransform>();
            m_PanelLayer = m_DropdownPanel.GetChildByName<RectTransform>("PanelLayer");
            m_DropdownCanvasGroup = m_DropdownPanel.GetComponent<CanvasGroup>();
            m_ShadowCanvasGroup = m_DropdownPanel.GetChildByName<CanvasGroup>("Shadow");

            m_CancelPanel = m_DropdownPanel.GetChildByName<RectTransform>("Cancel Panel");
            m_CancelPanel.sizeDelta = MaterialUIScaler.GetParentScaler(m_CancelPanel).targetCanvas.pixelRect.size * 2;
            DropdownTrigger trigger = m_DropdownPanel.gameObject.GetChildByName<DropdownTrigger>("Cancel Panel");
            trigger.index = -1;
            trigger.dropdown = this;

            m_DropdownPanel.gameObject.GetChildByName<Image>("ScrollRect").color = m_PanelColor;

            m_ListItemTemplate = new DropdownListItem();

            m_ListItemTemplate.rectTransform = m_DropdownPanel.GetChildByName<RectTransform>("Item");
            m_ListItemTemplate.canvasGroup = m_ListItemTemplate.rectTransform.GetComponent<CanvasGroup>();
            m_ListItemTemplate.text = m_ListItemTemplate.rectTransform.GetChildByName<Text>("Text");

            if (m_OptionDataList.imageType == ImageDataType.Sprite)
            {
                m_ListItemTemplate.image = m_ListItemTemplate.rectTransform.GetChildByName<Image>("Icon");
                Destroy(m_ListItemTemplate.rectTransform.GetChildByName<VectorImage>("Icon").gameObject);
            }
            else
            {
                m_ListItemTemplate.image = m_ListItemTemplate.rectTransform.GetChildByName<VectorImage>("Icon");
                Destroy(m_ListItemTemplate.rectTransform.GetChildByName<Image>("Icon").gameObject);
            }

            m_ListItems = new List<DropdownListItem>();

            for (int i = 0; i < m_OptionDataList.options.Count; i++)
            {
                m_ListItems.Add(CreateItem(m_OptionDataList.options[i], i));
            }

            for (int i = 0; i < m_ListItems.Count; i++)
            {
                Selectable selectable = m_ListItems[i].rectTransform.GetComponent<Selectable>();
                Navigation navigation = new Navigation();
                navigation.mode = Navigation.Mode.Explicit;

                if (i > 0)
                {
                    navigation.selectOnUp = m_ListItems[i - 1].rectTransform.GetComponent<Selectable>();
                }
                if (i < m_ListItems.Count - 1)
                {
                    navigation.selectOnDown = m_ListItems[i + 1].rectTransform.GetComponent<Selectable>();
                }

                selectable.navigation = navigation;
            }

            if (m_BaseSelectable != null)
            {
                if (m_ListItems.Count > 0)
                {
                    Navigation navigation = Navigation.defaultNavigation;
                    navigation.selectOnDown = m_ListItems[0].rectTransform.GetComponent<Selectable>();
                    m_BaseSelectable.navigation = navigation;
                }
            }

            float maxWidth = CalculateMaxItemWidth();
            float buttonWidth = m_BaseTransform.rect.width;

            m_FullHeight = m_OptionDataList.options.Count * LayoutUtility.GetPreferredHeight(m_ListItemTemplate.rectTransform) + 16;

            m_ExpandedSize = new Vector2(Mathf.Max(maxWidth, buttonWidth), m_FullHeight);

            m_TempMaxHeight = m_MaxHeight;

            if (m_TempMaxHeight == 0)
            {
                m_TempMaxHeight = MaterialUIScaler.GetParentScaler(m_DropdownPanel).targetCanvas.GetComponent<RectTransform>().rect.height - 32;
            }

            if (m_ExpandedSize.y > m_TempMaxHeight)
            {
                m_ExpandedSize.y = m_TempMaxHeight;
            }
            else
            {
                m_DropdownPanel.GetChildByName<Image>("Handle").gameObject.SetActive(false);
            }

            Destroy(m_ListItemTemplate.rectTransform.gameObject);

            m_DropdownPanel.position = m_BaseTransform.GetPositionRegardlessOfPivot();

            if (m_ExpandStartType == ExpandStartType.ExpandFromBaseTransformWidth)
            {
                if (m_VerticalPivotType == VerticalPivotType.BelowBase)
                {
                    m_DropdownPanel.position = new Vector3(m_DropdownPanel.position.x, m_DropdownPanel.position.y - m_BaseTransform.GetProperSize().y / 2, m_DropdownPanel.position.z);
                }
                else if (m_VerticalPivotType == VerticalPivotType.AboveBase)
                {
                    m_DropdownPanel.position = new Vector3(m_DropdownPanel.position.x, m_DropdownPanel.position.y + m_BaseTransform.GetProperSize().y / 2, m_DropdownPanel.position.z);
                }
            }

            m_ExpandedPosition = CalculatedPosition();

            m_DropdownCanvasGroup.alpha = 0f;
            m_ShadowCanvasGroup.alpha = 0f;

            if (m_ExpandStartType == ExpandStartType.ExpandFromBaseTransformWidth)
            {
                m_DropdownPanel.sizeDelta = new Vector2(m_BaseTransform.rect.size.x, 0f);
            }
            else if (m_ExpandStartType == ExpandStartType.ExpandFromBaseTransformHeight)
            {
                m_DropdownPanel.sizeDelta = new Vector2(0f, m_BaseTransform.rect.size.y);
            }
            else if (m_ExpandStartType == ExpandStartType.ExpandFromBaseTransformSize)
            {
                m_DropdownPanel.sizeDelta = m_BaseTransform.rect.size;
            }
            else
            {
                m_DropdownPanel.sizeDelta = Vector2.zero;
            }

            m_DropdownPanel.gameObject.SetActive(true);

            for (int i = 0; i < m_ListItemAutoTweeners.Count; i++)
            {
                TweenManager.EndTween(m_ListItemAutoTweeners[i]);
            }

            m_AutoTweeners = new List<int>();
            m_ListItemAutoTweeners = new List<int>();

            m_AutoTweeners.Add(TweenManager.TweenFloat(f => m_DropdownCanvasGroup.alpha = f, m_DropdownCanvasGroup.alpha, 1f, m_AnimationDuration * 0.66f, 0, null, false, Tween.TweenType.Linear));
            m_AutoTweeners.Add(TweenManager.TweenFloat(f => m_ShadowCanvasGroup.alpha = f, m_ShadowCanvasGroup.alpha, 1f, m_AnimationDuration * 0.66f, 0, null, false, Tween.TweenType.Linear));

            m_AutoTweeners.Add(TweenManager.TweenVector2(vector2 => m_DropdownPanel.sizeDelta = vector2, m_DropdownPanel.sizeDelta, m_ExpandedSize, m_AnimationDuration, m_AnimationDuration / 3, null, false, Tween.TweenType.EaseInOutQuint));

            m_AutoTweeners.Add(TweenManager.TweenVector2(vector2 => m_DropdownPanel.position = vector2, m_DropdownPanel.position, m_ExpandedPosition, m_AnimationDuration, m_AnimationDuration / 3, () =>
            {
                if (m_BaseSelectable != null && m_IsExapanded)
                {
                    m_BaseSelectable.interactable = false;
                }

                Vector2 tempVector2 = m_PanelLayer.anchoredPosition;
                tempVector2.x = Mathf.RoundToInt(tempVector2.x);
                tempVector2.y = Mathf.RoundToInt(tempVector2.y);
                m_PanelLayer.anchoredPosition = tempVector2;
            }, false, Tween.TweenType.EaseInOutQuint));

            for (int i = 0; i < m_ListItems.Count; i++)
            {
                int i1 = i;
                CanvasGroup canvasGroup = m_ListItems[i].canvasGroup;
                m_ListItemAutoTweeners.Add(TweenManager.TweenFloat(f => canvasGroup.alpha = f, canvasGroup.alpha, 1f, m_AnimationDuration * 1.66f, (i1 * (m_AnimationDuration / 6) + m_AnimationDuration) - m_ScrollPosOffset / 800, null, false, Tween.TweenType.Linear));
            }

            if (m_FullHeight > m_TempMaxHeight)
            {
                m_DropdownPanel.GetChildByName<ScrollRect>("ScrollRect").gameObject.AddComponent<RectMask2D>();
            }

            m_IsExapanded = true;

            m_TimeShown = Time.unscaledTime;
        }

        public void Hide()
        {
            for (int i = 0; i < m_ListItemAutoTweeners.Count; i++)
            {
                TweenManager.EndTween(m_ListItemAutoTweeners[i]);
            }

            m_IsExapanded = false;

            if (m_BaseSelectable != null)
            {
                m_BaseSelectable.interactable = true;
            }

            for (int i = 0; i < m_ListItems.Count; i++)
            {
                int i1 = i;
                CanvasGroup canvasGroup = m_ListItems[i].canvasGroup;
                TweenManager.TweenFloat(f => canvasGroup.alpha = f, canvasGroup.alpha, 0f, m_AnimationDuration * 0.66f, (m_ListItems.Count - i1) * (m_AnimationDuration / 6), null, false, Tween.TweenType.Linear);
            }

            m_AutoTweeners.Add(TweenManager.TweenFloat(f => m_DropdownCanvasGroup.alpha = f, m_DropdownCanvasGroup.alpha, 0f, m_AnimationDuration * 0.66f, m_AnimationDuration, null, false, Tween.TweenType.Linear));

            TweenManager.TweenFloat(f => m_ShadowCanvasGroup.alpha = f, m_ShadowCanvasGroup.alpha, 0f, m_AnimationDuration * 0.66f, m_AnimationDuration, () =>
            {
                for (int i = 0; i < m_AutoTweeners.Count; i++)
                {
                    TweenManager.EndTween(m_AutoTweeners[i]);
                }

                Destroy(m_DropdownPanel.gameObject);
            }, false, Tween.TweenType.Linear);
        }

        public void Select(int selectedItem, bool submitted = false)
        {
            if (Time.unscaledTime - m_TimeShown < m_IgnoreInputAfterShowTimer) return;

            if (!m_IsExapanded) return;

            if (selectedItem >= 0)
            {
                if (m_ButtonImageContent != null && m_UpdateHeader)
                {
                    m_ButtonImageContent.SetImage(m_OptionDataList.options[selectedItem].imageData);
                }

                if (m_ButtonTextContent != null && m_UpdateHeader)
                {
                    string itemText = m_OptionDataList.options[selectedItem].text;

                    if (m_CapitalizeButtonText)
                    {
                        itemText = itemText.ToUpper();
                    }

                    m_ButtonTextContent.text = itemText;
                }

                m_CurrentlySelected = selectedItem;
            }

            Hide();

            if (submitted && m_BaseSelectable != null)
            {
                EventSystem.current.SetSelectedGameObject(m_BaseSelectable.gameObject);
            }

            if (m_OnItemSelected != null)
            {
                m_OnItemSelected.Invoke(selectedItem);
            }

            if (selectedItem >= 0 && selectedItem < m_OptionDataList.options.Count)
            {
                m_OptionDataList.options[selectedItem].onOptionSelected.InvokeIfNotNull();
            }
        }

        private Vector3 CalculatedPosition()
        {
            Vector3 position = m_BaseTransform.GetPositionRegardlessOfPivot();
            float itemHeight = m_ListItemTemplate.rectTransform.GetProperSize().y;
            float minScrollPos = 0f;
            float maxScrollPos = Mathf.Clamp(m_FullHeight - m_TempMaxHeight, 0f, float.MaxValue);

            int flipper = (int)m_VerticalPivotType < 3 ? 1 : -1;

            if (m_VerticalPivotType == VerticalPivotType.BelowBase || m_VerticalPivotType == VerticalPivotType.AboveBase)
            {
                float baseHeight = m_BaseTransform.GetProperSize().y;

                position.y -= (m_ExpandedSize.y / 2) * flipper;
                position.y -= (baseHeight / 2) * flipper;
            }
            else if (m_VerticalPivotType == VerticalPivotType.Top || m_VerticalPivotType == VerticalPivotType.Bottom)
            {
                position.y -= (m_ExpandedSize.y / 2) * flipper;
                position.y += (itemHeight / 2) * flipper;
                position.y -= (4) * flipper;
            }
            else if (m_VerticalPivotType == VerticalPivotType.FirstItem || m_VerticalPivotType == VerticalPivotType.LastItem)
            {
                position.y -= (m_ExpandedSize.y / 2) * flipper;
                position.y += (itemHeight / 2) * flipper;
                position.y += (8) * flipper;
            }

            if (m_HighlightCurrentlySelected)
            {
                Vector2 tempVector2 = m_PanelLayer.anchoredPosition;
                tempVector2.y += itemHeight * Mathf.Clamp(m_CurrentlySelected, 0, int.MaxValue);
                if (m_VerticalPivotType == VerticalPivotType.Center)
                {
                    tempVector2.y -= m_ExpandedSize.y / 2;
                    tempVector2.y += itemHeight / 2;
                    tempVector2.y += 8;
                }
                else if (m_VerticalPivotType == VerticalPivotType.LastItem)
                {
                    tempVector2.y -= m_ExpandedSize.y;
                    tempVector2.y += itemHeight;
                    tempVector2.y += 16;
                }
                tempVector2.y = Mathf.Clamp(tempVector2.y, minScrollPos, maxScrollPos);
                m_PanelLayer.anchoredPosition = tempVector2;

                m_ScrollPosOffset = tempVector2.y;
            }
            else
            {
                m_ScrollPosOffset = 0;
            }

            flipper = m_HorizontalPivotType == HorizontalPivotType.Left ? 1 : -1;

            if (m_HorizontalPivotType != HorizontalPivotType.Center)
            {
                position.x -= (m_BaseTransform.GetProperSize().x / 2) * flipper;
                position.x += (m_ExpandedSize.x / 2) * flipper;
            }

            RectTransform rootCanvasRectTransform = MaterialUIScaler.GetParentScaler(m_DropdownPanel).GetComponent<RectTransform>();

            //  Left edge
            float canvasEdge = rootCanvasRectTransform.position.x - rootCanvasRectTransform.rect.width / 2;
            float dropdownEdge = position.x - m_ExpandedSize.x / 2;
            if (dropdownEdge < canvasEdge + m_MinDistanceFromEdge)
            {
                position.x += (canvasEdge + m_MinDistanceFromEdge) - dropdownEdge;
            }

            //  Right edge
            canvasEdge = rootCanvasRectTransform.position.x + rootCanvasRectTransform.rect.width / 2;
            dropdownEdge = position.x + m_ExpandedSize.x / 2;
            if (dropdownEdge > canvasEdge - m_MinDistanceFromEdge)
            {
                position.x -= dropdownEdge - (canvasEdge - m_MinDistanceFromEdge);
            }

            //  Top edge
            canvasEdge = rootCanvasRectTransform.position.y + rootCanvasRectTransform.rect.height / 2;
            dropdownEdge = position.y + m_ExpandedSize.y / 2;
            if (dropdownEdge > canvasEdge - m_MinDistanceFromEdge)
            {
                position.y -= dropdownEdge - (canvasEdge - m_MinDistanceFromEdge);
            }

            //  Bottom edge
            canvasEdge = rootCanvasRectTransform.position.y - rootCanvasRectTransform.rect.height / 2;
            dropdownEdge = position.y - m_ExpandedSize.y / 2;
            if (dropdownEdge < canvasEdge + m_MinDistanceFromEdge)
            {
                position.y += (canvasEdge + m_MinDistanceFromEdge) - dropdownEdge;
            }

            return position;
        }

        private DropdownListItem CreateItem(OptionData data, int index)
        {
            DropdownListItem item = new DropdownListItem();

            GameObject itemGameObject = Instantiate(m_ListItemTemplate.rectTransform.gameObject);

            item.rectTransform = itemGameObject.GetComponent<RectTransform>();

            item.rectTransform.SetParent(m_ListItemTemplate.rectTransform.parent);
            item.rectTransform.localScale = Vector3.one;
            item.rectTransform.localEulerAngles = Vector3.zero;
            item.rectTransform.anchoredPosition3D = Vector3.zero;

            item.canvasGroup = item.rectTransform.GetComponent<CanvasGroup>();
            item.text = item.rectTransform.GetChildByName<Text>("Text");

            if (m_OptionDataList.imageType == ImageDataType.Sprite)
            {
                item.image = item.rectTransform.GetChildByName<Image>("Icon");
                Destroy(item.rectTransform.GetChildByName<VectorImage>("Icon").gameObject);
            }
            else
            {
                item.image = item.rectTransform.GetChildByName<VectorImage>("Icon");
                Destroy(item.rectTransform.GetChildByName<Image>("Icon").gameObject);
            }

            DropdownTrigger trigger = itemGameObject.GetComponent<DropdownTrigger>();
            trigger.index = index;
            trigger.dropdown = this;

            if (!string.IsNullOrEmpty(data.text))
            {
                item.text.text = data.text;
            }
            else
            {
                Destroy(item.text.gameObject);
            }

            if (data.imageData != null && data.imageData.ContainsData(true))
            {
                item.image.SetImage(data.imageData);
            }
            else
            {
                Destroy(item.image.gameObject);
            }

            itemGameObject.GetComponent<MaterialRipple>().rippleData = m_ItemRippleData.Copy();

            if (m_HighlightCurrentlySelected && index == m_CurrentlySelected)
            {
                itemGameObject.GetComponent<Image>().color = m_ItemRippleData.Color.WithAlpha(m_ItemRippleData.EndAlpha);
            }

            item.text.color = m_ItemTextColor;
            item.image.color = m_ItemIconColor;

            item.canvasGroup.alpha = 0f;

            return item;
        }

        private float CalculateMaxItemWidth()
        {
            TextGenerator textGenerator = new TextGenerator();
            TextGenerationSettings textGenerationSettings = m_ListItemTemplate.text.GetGenerationSettings(new Vector2(float.MaxValue, float.MaxValue));

            float maxWidth = 0f;

            for (int i = 0; i < m_OptionDataList.options.Count; i++)
            {
                float currentWidth = 0f;

                if (!string.IsNullOrEmpty(m_OptionDataList.options[i].text))
                {
                    currentWidth = textGenerator.GetPreferredWidth(m_OptionDataList.options[i].text, textGenerationSettings);
                    currentWidth += 16;
                }

                if (m_OptionDataList.imageType == ImageDataType.Sprite)
                {
                    if (m_OptionDataList.options[i].imageData.sprite != null)
                    {
                        currentWidth += m_ListItemTemplate.image.rectTransform.rect.width;
                        currentWidth += 16;
                    }
                }
                else
                {
                    if (m_OptionDataList.options[i].imageData != null && m_OptionDataList.options[i].imageData.vectorImageData != null)
                    {
                        currentWidth += m_ListItemTemplate.image.rectTransform.rect.width;
                        currentWidth += 16;
                    }
                }



                currentWidth += 16;

                maxWidth = Mathf.Max(maxWidth, currentWidth);
            }

            return maxWidth;
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            for (int i = 0; i < m_OptionDataList.options.Count; i++)
            {
                m_OptionDataList.options[i].imageData.imageDataType = m_OptionDataList.imageType;
            }

            m_CurrentlySelected = Mathf.Clamp(m_CurrentlySelected, -1, m_OptionDataList.options.Count - 1);

            if (m_CurrentlySelected >= 0)
            {
                if (m_ButtonImageContent != null && m_UpdateHeader)
                {
                    m_ButtonImageContent.SetImage(m_OptionDataList.options[m_CurrentlySelected].imageData);
                }

                if (m_ButtonTextContent != null && m_UpdateHeader)
                {
                    string itemText = m_OptionDataList.options[m_CurrentlySelected].text;

                    if (m_CapitalizeButtonText)
                    {
                        itemText = itemText.ToUpper();
                    }

                    m_ButtonTextContent.text = itemText;
                }
            }
        }
#endif
    }
}