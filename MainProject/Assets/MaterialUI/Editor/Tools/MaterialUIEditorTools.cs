//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace MaterialUI
{
    [ExecuteInEditMode]
    public static class MaterialUIEditorTools
    {
        private static GameObject m_LastInstance;
        private static GameObject m_SelectedObject;
        private static bool m_NotCanvas;

        #region GeneralCreationMethods

        public static void CreateInstance(string assetPath, string objectName, params InstantiationOptions[] instantiationOptions)
        {
            m_LastInstance = Object.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/Prefabs/" + assetPath + ".prefab", typeof(GameObject))) as GameObject;
            m_LastInstance.name = objectName;

            CreateCanvasIfNeeded();

            m_LastInstance.transform.SetParent(m_SelectedObject.transform);
            m_LastInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            m_LastInstance.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            Selection.activeObject = m_LastInstance;

            if (instantiationOptions.Length > 0)
            { 
				GameObject savedLastInstance = m_LastInstance;

				InstantiationHelper instantiationHelper = m_LastInstance.GetComponent<InstantiationHelper>();
				if (instantiationHelper == null)
				{
					instantiationHelper = m_LastInstance.GetComponentInChildren<InstantiationHelper>();

					m_LastInstance = instantiationHelper.gameObject;
					Selection.activeObject = m_LastInstance;
				}

				instantiationHelper.HelpInstantiate(instantiationOptions);

				if (savedLastInstance != null)
				{
					Selection.activeObject = savedLastInstance;
				}
            }

            Undo.RegisterCreatedObjectUndo(m_LastInstance, "create " + m_LastInstance.name);
        }

        private static void CreateCanvasIfNeeded()
        {
            if (Selection.activeObject != null && Selection.activeObject.GetType() == (typeof(GameObject)))
            {
                m_SelectedObject = (GameObject)Selection.activeObject;
            }

            if (m_SelectedObject)
            {
                if (GameObject.Find(m_SelectedObject.name))
                {
                    m_NotCanvas = m_SelectedObject.GetComponentInParent<Canvas>() == null;
                }
                else
                {
                    m_NotCanvas = true;
                }
            }
            else
            {
                m_NotCanvas = true;
            }

            if (m_NotCanvas)
            {
                if (!Object.FindObjectOfType<UnityEngine.EventSystems.EventSystem>())
                {
                    Object.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/Prefabs/Common/EventSystem.prefab", typeof(GameObject))).name = "EventSystem";
                }

                Canvas[] canvases = Object.FindObjectsOfType<Canvas>();

                for (int i = 0; i < canvases.Length; i++)
                {
                    if (canvases[i].isRootCanvas)
                    {
                        m_SelectedObject = canvases[i].gameObject;
                    }
                }
                if (!m_SelectedObject)
                {
                    m_SelectedObject = Object.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/MaterialUI/Prefabs/Common/Canvas.prefab", typeof(GameObject))) as GameObject;
                    m_SelectedObject.name = "Canvas";
                }
            }
        }

        #endregion

        #region CreateObjects

        [MenuItem("GameObject/MaterialUI/Empty Object/Self Sized/No Layout", false, 000)]
        private static void CreateEmptyObjectSelfSizedNoLayout()
        {
            CreateInstance("Components/Empty Object", "Empty Object", InstantiationOptions.None);
        }

        [MenuItem("GameObject/MaterialUI/Empty Object/Self Sized/Horizontal Layout", false, 000)]
        private static void CreateEmptyObjectSelfSizedHorizontalLayout()
        {
            CreateInstance("Components/Empty Object", "Empty Object", InstantiationOptions.HasLayoutHorizontal);
        }

        [MenuItem("GameObject/MaterialUI/Empty Object/Self Sized/Vertical Layout", false, 000)]
        private static void CreateEmptyObjectSelfSizedVerticalLayout()
        {
            CreateInstance("Components/Empty Object", "Empty Object", InstantiationOptions.HasLayoutVertical);
        }

        [MenuItem("GameObject/MaterialUI/Empty Object/Stretched/No Layout", false, 000)]
        private static void CreateEmptyObjectStretchedNoLayout()
        {
            CreateInstance("Components/Empty Object", "Empty Object", InstantiationOptions.HasContent);
        }

        [MenuItem("GameObject/MaterialUI/Empty Object/Stretched/Horizontal Layout", false, 000)]
        private static void CreateEmptyObjectStretchedHorizontalLayout()
        {
            CreateInstance("Components/Empty Object", "Empty Object", InstantiationOptions.HasContent, InstantiationOptions.HasLayoutHorizontal);
        }

        [MenuItem("GameObject/MaterialUI/Empty Object/Stretched/Vertical Layout", false, 000)]
        private static void CreateEmptyObjectStretchedVerticalLayout()
        {
            CreateInstance("Components/Empty Object", "Empty Object", InstantiationOptions.HasContent, InstantiationOptions.HasLayoutVertical);
        }

        [MenuItem("GameObject/MaterialUI/Empty Object/Fitted/Horizontal Layout", false, 000)]
        private static void CreateEmptyObjectFittedHorizontalLayout()
        {
            CreateInstance("Components/Empty Object", "Empty Object", InstantiationOptions.Fitted, InstantiationOptions.HasLayoutHorizontal);
        }

        [MenuItem("GameObject/MaterialUI/Empty Object/Fitted/Vertical Layout", false, 000)]
        private static void CreateEmptyObjectFittedVerticalLayout()
        {
            CreateInstance("Components/Empty Object", "Empty Object", InstantiationOptions.Fitted, InstantiationOptions.HasLayoutVertical);
        }

        [MenuItem("GameObject/MaterialUI/Panel/Self Sized/No Layout", false, 005)]
        private static void CreatePanelSelfSizedNoLayout()
        {
            CreateInstance("Components/Panel", "Panel", InstantiationOptions.None);
        }

        [MenuItem("GameObject/MaterialUI/Panel/Self Sized/Horizontal Layout", false, 001)]
        private static void CreatePanelSelfSizedHorizontalLayout()
        {
            CreateInstance("Components/Panel", "Panel", InstantiationOptions.HasLayoutHorizontal);
        }

        [MenuItem("GameObject/MaterialUI/Panel/Self Sized/Vertical Layout", false, 001)]
        private static void CreatePanelSelfSizedVerticalLayout()
        {
            CreateInstance("Components/Panel", "Panel", InstantiationOptions.HasLayoutVertical);
        }

        [MenuItem("GameObject/MaterialUI/Panel/Stretched/No Layout", false, 001)]
        private static void CreatePanelStretchedNoLayout()
        {
            CreateInstance("Components/Panel", "Panel", InstantiationOptions.HasContent);
        }

        [MenuItem("GameObject/MaterialUI/Panel/Stretched/Horizontal Layout", false, 001)]
        private static void CreatePanelStretchedHorizontalLayout()
        {
            CreateInstance("Components/Panel", "Panel", InstantiationOptions.HasContent, InstantiationOptions.HasLayoutHorizontal);
        }

        [MenuItem("GameObject/MaterialUI/Panel/Stretched/Vertical Layout", false, 001)]
        private static void CreatePanelStretchedVerticalLayout()
        {
            CreateInstance("Components/Panel", "Panel", InstantiationOptions.HasContent, InstantiationOptions.HasLayoutVertical);
        }

        [MenuItem("GameObject/MaterialUI/Panel/Fitted/Horizontal Layout", false, 001)]
        private static void CreatePanelFittedHorizontalLayout()
        {
            CreateInstance("Components/Panel", "Panel", InstantiationOptions.Fitted, InstantiationOptions.HasLayoutHorizontal);
        }

        [MenuItem("GameObject/MaterialUI/Panel/Fitted/Vertical Layout", false, 001)]
        private static void CreatePanelFittedVerticalLayout()
        {
            CreateInstance("Components/Panel", "Panel", InstantiationOptions.Fitted, InstantiationOptions.HasLayoutVertical);
        }

        [MenuItem("GameObject/MaterialUI/Background Image", false, 010)]
        private static void CreateBackgroundImage()
        {
            CreateInstance("Components/Background Image", "Background Image");
            m_LastInstance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        }

        [MenuItem("GameObject/MaterialUI/Image", false, 010)]
        private static void CreateImage()
        {
            CreateInstance("Components/Image", "Image");
        }

        //[MenuItem("GameObject/MaterialUI/Shadow", false, 011)]
        private static void CreateShadow()
        {
            CreateInstance("Components/Shadow", "Shadow");
        }

        [MenuItem("GameObject/MaterialUI/Vector Image", false, 014)]
        private static void CreateVectorImage()
        {
            CreateInstance("Components/VectorImage", "Icon");
        }

        [MenuItem("GameObject/MaterialUI/Text", false, 020)]
        private static void CreateText()
        {
            CreateInstance("Components/Text", "Text");
        }

        [MenuItem("GameObject/MaterialUI/Buttons/Text/Flat", false, 030)]
        private static void CreateButtonFlat()
        {
            CreateInstance("Components/Buttons/Button", "Button - Flat", InstantiationOptions.None);
        }

        [MenuItem("GameObject/MaterialUI/Buttons/Text/Raised", false, 030)]
        private static void CreateButtonRaised()
        {
            CreateInstance("Components/Buttons/Button", "Button - Raised", InstantiationOptions.Raised);
        }

        [MenuItem("GameObject/MaterialUI/Buttons/Multi Content/Flat", false, 030)]
        private static void CreateButtonMultiFlat()
        {
            CreateInstance("Components/Buttons/Button", "Button - Flat", InstantiationOptions.HasContent);
        }

        [MenuItem("GameObject/MaterialUI/Buttons/Multi Content/Raised", false, 030)]
        private static void CreateButtonMultiRaised()
        {
            CreateInstance("Components/Buttons/Button", "Button - Raised", InstantiationOptions.HasContent, InstantiationOptions.Raised);
        }

        [MenuItem("GameObject/MaterialUI/Buttons/Floating Action Button/Raised", false, 030)]
        private static void CreateFloatingActionButtonRaised()
        {
            CreateInstance("Components/Buttons/Floating Action Button", "Floating Action Button - Raised", InstantiationOptions.Raised);
        }

        [MenuItem("GameObject/MaterialUI/Buttons/Floating Action Button/Mini Raised", false, 030)]
        private static void CreateMiniFloatingActionButtonRaised()
        {
            CreateInstance("Components/Buttons/Floating Action Button", "Floating Action Button Mini - Raised", InstantiationOptions.Mini, InstantiationOptions.Raised);
        }

        [MenuItem("GameObject/MaterialUI/Buttons/Icon Button/Normal", false, 030)]
        private static void CreateIconButton()
        {
            CreateInstance("Components/Buttons/Floating Action Button", "Icon Button - Flat", InstantiationOptions.None);
        }

        [MenuItem("GameObject/MaterialUI/Buttons/Icon Button/Mini", false, 030)]
        private static void CreateMiniIconButton()
        {
            CreateInstance("Components/Buttons/Floating Action Button", "Icon Button Mini - Flat", InstantiationOptions.Mini);
        }

        [MenuItem("GameObject/MaterialUI/Dropdowns/Flat", false, 030)]
        private static void CreateDropdownFlat()
        {
            CreateInstance("Components/Buttons/Button", "Dropdown - Flat", InstantiationOptions.HasDropdown, InstantiationOptions.HasContent);
        }

        [MenuItem("GameObject/MaterialUI/Dropdowns/Raised", false, 030)]
        private static void CreateDropdownRaised()
        {
            CreateInstance("Components/Buttons/Button", "Dropdown - Raised", InstantiationOptions.HasDropdown, InstantiationOptions.HasContent, InstantiationOptions.Raised);
        }

        [MenuItem("GameObject/MaterialUI/Dropdowns/Icon Button", false, 030)]
        private static void CreateIconDropdown()
        {
            CreateInstance("Components/Buttons/Floating Action Button", "Dropdown Icon Button", InstantiationOptions.HasDropdown);
        }

        [MenuItem("GameObject/MaterialUI/Dropdowns/Mini Icon Button", false, 030)]
        private static void CreateMiniIconDropdown()
        {
            CreateInstance("Components/Buttons/Floating Action Button", "Dropdown Mini Icon Button", InstantiationOptions.HasDropdown, InstantiationOptions.Mini);
        }

        [MenuItem("GameObject/MaterialUI/Selection Controls/Checkboxes/Label", false, 040)]
        private static void CreateCheckboxText()
        {
            CreateInstance("Components/Checkbox", "Checkbox", InstantiationOptions.Label);
        }

        [MenuItem("GameObject/MaterialUI/Selection Controls/Checkboxes/Icon", false, 040)]
        private static void CreateCheckboxIcon()
        {
            CreateInstance("Components/Checkbox", "Checkbox", InstantiationOptions.Icon);
        }

        [MenuItem("GameObject/MaterialUI/Selection Controls/Switches/Label", false, 050)]
        private static void CreateSwitchLabel()
        {
            CreateInstance("Components/Switch", "Switch", InstantiationOptions.Label);
        }

        [MenuItem("GameObject/MaterialUI/Selection Controls/Switches/Icon", false, 050)]
        private static void CreateSwitchIcon()
        {
            CreateInstance("Components/Switch", "Switch", InstantiationOptions.Icon);
        }

        [MenuItem("GameObject/MaterialUI/Selection Controls/Radio Buttons/Label", false, 060)]
        private static void CreateRadioButtonsLabel()
        {
            CreateInstance("Components/RadioGroup", "Radio Buttons", InstantiationOptions.Label);
        }

        [MenuItem("GameObject/MaterialUI/Selection Controls/Radio Buttons/Icon", false, 060)]
        private static void CreateRadioButtonsIcon()
        {
            CreateInstance("Components/RadioGroup", "Radio Buttons", InstantiationOptions.Icon);
        }

        [MenuItem("GameObject/MaterialUI/Input Fields/Basic", false, 070)]
        private static void CreateSimpleInputFieldBasic()
        {
            CreateInstance("Components/Input Field", "Input Field", InstantiationOptions.None);
        }

        [MenuItem("GameObject/MaterialUI/Input Fields/Basic - With Icon", false, 070)]
        private static void CreateSimpleInputFieldIcon()
        {
            CreateInstance("Components/Input Field", "Input Field", InstantiationOptions.Icon);
        }

        [MenuItem("GameObject/MaterialUI/Input Fields/Basic - With Clear Button", false, 070)]
        private static void CreateSimpleInputFieldClearButton()
        {
            CreateInstance("Components/Input Field", "Input Field", InstantiationOptions.HasContent);
        }

        [MenuItem("GameObject/MaterialUI/Input Fields/Basic - With Icon and Clear Button", false, 070)]
        private static void CreateSimpleInputFieldIconClearButton()
        {
            CreateInstance("Components/Input Field", "Input Field", InstantiationOptions.Icon, InstantiationOptions.HasContent);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Continuous/Simple", false, 080)]
        private static void CreateSliderContinuousSimple()
        {
            CreateInstance("Components/Slider", "Slider - Simple", InstantiationOptions.None);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Continuous/Left label", false, 080)]
        private static void CreateSliderContinuousLabel()
        {
            CreateInstance("Components/Slider", "Slider - Left Label", InstantiationOptions.Label);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Continuous/Left icon", false, 080)]
        private static void CreateSliderContinuousIcon()
        {
            CreateInstance("Components/Slider", "Slider - Left icon", InstantiationOptions.Icon);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Continuous/Left and Right labels", false, 080)]
        private static void CreateSliderContinuousLabels()
        {
            CreateInstance("Components/Slider", "Slider - Left and Right labels", InstantiationOptions.Label, InstantiationOptions.HasContent);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Continuous/Left label and Right inputField", false, 080)]
        private static void CreateSliderContinuousLabelInputField()
        {
            CreateInstance("Components/Slider", "Slider - Left label and Right inputField", InstantiationOptions.Label, InstantiationOptions.HasInputField);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Continuous/Left icon and Right inputField", false, 080)]
        private static void CreateSliderContinuousIconInputField()
        {
            CreateInstance("Components/Slider", "Slider - Left icon and Right inputField", InstantiationOptions.Icon, InstantiationOptions.HasInputField);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Discrete/Simple", false, 080)]
        private static void CreateSliderDiscreteSimple()
        {
            CreateInstance("Components/Slider", "Slider - Simple", InstantiationOptions.Discrete);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Discrete/Left label", false, 080)]
        private static void CreateSliderDiscreteLabel()
        {
            CreateInstance("Components/Slider", "Slider - Left Label", InstantiationOptions.Discrete, InstantiationOptions.Label);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Discrete/Left icon", false, 080)]
        private static void CreateSliderDiscreteIcon()
        {
            CreateInstance("Components/Slider", "Slider - Left icon", InstantiationOptions.Discrete, InstantiationOptions.Icon);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Discrete/Left and Right labels", false, 080)]
        private static void CreateSliderDiscreteLabels()
        {
            CreateInstance("Components/Slider", "Slider - Left and Right labels", InstantiationOptions.Discrete, InstantiationOptions.Label, InstantiationOptions.HasContent);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Discrete/Left label and Right inputField", false, 080)]
        private static void CreateSliderDiscreteLabelInputField()
        {
            CreateInstance("Components/Slider", "Slider - Left label and Right inputField", InstantiationOptions.Discrete, InstantiationOptions.Label, InstantiationOptions.HasInputField);
        }

        [MenuItem("GameObject/MaterialUI/Sliders/Discrete/Left icon and Right inputField", false, 080)]
        private static void CreateSliderDiscreteIconInputField()
        {
            CreateInstance("Components/Slider", "Slider - Left icon and Right inputField", InstantiationOptions.Discrete, InstantiationOptions.Icon, InstantiationOptions.HasInputField);
        }

        [MenuItem("GameObject/MaterialUI/Progress Indicators/Simple/Circle - Flat", false, 082)]
        private static void CreateProgressCircleFlat()
        {
            CreateInstance("Resources/Progress Indicators/Circle Progress Indicator", "Circle Progress Indicator - Flat", InstantiationOptions.None);
        }

        [MenuItem("GameObject/MaterialUI/Progress Indicators/Simple/Circle - Raised", false, 082)]
        private static void CreateProgressCircleRaised()
        {
            CreateInstance("Resources/Progress Indicators/Circle Progress Indicator", "Circle Progress Indicator - Raised", InstantiationOptions.Raised);
        }

        [MenuItem("GameObject/MaterialUI/Progress Indicators/Simple/Linear", false, 082)]
        private static void CreateProgressLinear()
        {
            CreateInstance("Resources/Progress Indicators/Linear Progress Indicator", "Linear Progress Indicator");
        }

		[MenuItem("GameObject/MaterialUI/Progress Indicators/With label/Horizontal/Circle - Flat", false, 082)]
		private static void CreateProgressLabelCircleFlatHorizontal()
		{
			CreateInstance("Resources/Progress Indicators/Circle Progress Indicator", "Circle Progress Indicator - Flat", InstantiationOptions.None, InstantiationOptions.Label, InstantiationOptions.HasLayoutHorizontal);
		}

		[MenuItem("GameObject/MaterialUI/Progress Indicators/With label/Horizontal/Circle - Raised", false, 082)]
		private static void CreateProgressLabelCircleRaisedHorizontal()
		{
			CreateInstance("Resources/Progress Indicators/Circle Progress Indicator", "Circle Progress Indicator - Raised", InstantiationOptions.Raised, InstantiationOptions.Label, InstantiationOptions.HasLayoutHorizontal);
		}

		[MenuItem("GameObject/MaterialUI/Progress Indicators/With label/Vertical/Circle - Flat", false, 082)]
		private static void CreateProgressLabelCircleFlatVertical()
		{
			CreateInstance("Resources/Progress Indicators/Circle Progress Indicator", "Circle Progress Indicator - Flat", InstantiationOptions.None, InstantiationOptions.Label, InstantiationOptions.HasLayoutVertical);
		}

		[MenuItem("GameObject/MaterialUI/Progress Indicators/With label/Vertical/Circle - Raised", false, 082)]
		private static void CreateProgressLabelCircleRaisedVertical()
		{
			CreateInstance("Resources/Progress Indicators/Circle Progress Indicator", "Circle Progress Indicator - Flat", InstantiationOptions.Raised, InstantiationOptions.Label, InstantiationOptions.HasLayoutVertical);
		}

        [MenuItem("GameObject/MaterialUI/Dividers/Horizontal Light", false, 120)]
        private static void CreateDividerHorizontalLight()
        {
            CreateInstance("Components/Divider", "Divider - Horizontal Light", InstantiationOptions.Light);
        }

        [MenuItem("GameObject/MaterialUI/Dividers/Horizontal Dark", false, 120)]
        private static void CreateDividerHorizontalDark()
        {
            CreateInstance("Components/Divider", "Divider - Horizontal Dark", InstantiationOptions.None);
        }

        [MenuItem("GameObject/MaterialUI/Dividers/Vertical Light", false, 120)]
        private static void CreateDividerVerticalLight()
        {
            CreateInstance("Components/Divider", "Divider - Vertical Light", InstantiationOptions.Light, InstantiationOptions.Vertical);
        }

        [MenuItem("GameObject/MaterialUI/Dividers/Vertical Dark", false, 120)]
        private static void CreateDividerVerticalDark()
        {
            CreateInstance("Components/Divider", "Divider - Vertical Dark", InstantiationOptions.Vertical);
        }

        [MenuItem("GameObject/MaterialUI/Nav Drawer", false, 200)]
        private static void CreateNavDrawer()
        {
            CreateInstance("Components/Nav Drawer", "Nav Drawer");
            m_LastInstance.GetComponent<RectTransform>().sizeDelta = new Vector2(m_LastInstance.GetComponent<RectTransform>().sizeDelta.x, 8f);
            m_LastInstance.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(-m_LastInstance.GetComponent<RectTransform>().sizeDelta.x / 2f, 0f);
        }

        [MenuItem("GameObject/MaterialUI/App Bar", false, 210)]
        private static void CreateAppBar()
        {
            CreateInstance("Components/App Bar", "App Bar");
            m_LastInstance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            m_LastInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        [MenuItem("GameObject/MaterialUI/Tab View/Icon", false, 210)]
        private static void CreateTabViewIcon()
        {
            CreateInstance("Components/TabView", "Tab View", InstantiationOptions.Icon);
        }

        [MenuItem("GameObject/MaterialUI/Tab View/Text", false, 210)]
        private static void CreateTabViewText()
        {
            CreateInstance("Components/TabView", "Tab View", InstantiationOptions.Label);
        }

        [MenuItem("GameObject/MaterialUI/Tab View/Icon and Text", false, 210)]
        private static void CreateTabView()
        {
            CreateInstance("Components/TabView", "Tab View", InstantiationOptions.Icon, InstantiationOptions.Label);
        }

        [MenuItem("GameObject/MaterialUI/Screens/Screen View", false, 220)]
        private static void CreateScreenView()
        {
            CreateInstance("Components/ScreenView", "Screen View");
            m_LastInstance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            m_LastInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        [MenuItem("GameObject/MaterialUI/Screens/Screen", false, 220)]
        private static void CreateScreen()
        {
            CreateInstance("Components/Screen", "Screen");
            m_LastInstance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            m_LastInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        [MenuItem("GameObject/MaterialUI/Managers/Toast Manager", false, 1000)]
        private static void CreateToastManager()
        {
            CreateInstance("Managers/ToastManager", "Toast Manager");
            m_LastInstance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            m_LastInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        [MenuItem("GameObject/MaterialUI/Managers/Snackbar Manager", false, 1000)]
        private static void CreateSnackbarManager()
        {
            CreateInstance("Managers/SnackbarManager", "Snackbar Manager");
            m_LastInstance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            m_LastInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        [MenuItem("GameObject/MaterialUI/Managers/Dialog Manager", false, 1000)]
        private static void CreateDialogManager()
        {
            CreateInstance("Managers/DialogManager", "Dialog Manager");
            m_LastInstance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            m_LastInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        #endregion

        #region GeneralMenuItems

        [MenuItem("Help/MaterialUI/Website", false, 200)]
        private static void Wiki()
        {
            Application.OpenURL("https://materialunity.com");
        }

        [MenuItem("Help/MaterialUI/Feedback - Bug Report - Feature Request", false, 200)]
        private static void Feedback()
        {
            Application.OpenURL("http://materialunity.com/support");
        }

        [MenuItem("Help/MaterialUI/Current Version: v" + MaterialUIVersion.currentVersion, true, 200)]
        private static bool AboutValidate()
        {
            return false;
        }

        [MenuItem("Help/MaterialUI/Current Version: v" + MaterialUIVersion.currentVersion, false, 200)]
        private static void About() { }

        #endregion

        #region Tools

        [MenuItem("GameObject/MaterialUI/Tools/Attach and Setup Shadow", true, 3000)]
        public static bool CheckAttachAndSetupShadow()
        {
            if (Selection.activeGameObject != null)
            {
                return true;
            }
            return false;
        }

        [MenuItem("GameObject/MaterialUI/Tools/Attach and Setup Shadow", false, 3000)]
        public static void AttachAndSetupShadow()
        {
            GameObject sourceGameObject = Selection.activeGameObject;
            Undo.RecordObject(sourceGameObject, sourceGameObject.name);
            RectTransform sourceRectTransform = sourceGameObject.GetComponent<RectTransform>();
            Vector3 sourcePos = sourceRectTransform.position;
            Vector2 sourceSize = sourceRectTransform.sizeDelta;
            Vector2 sourceLayoutSize = sourceRectTransform.GetProperSize();
            Image sourceImage = sourceGameObject.GetAddComponent<Image>();

            CreateShadow();

            GameObject shadowGameObject = Selection.activeGameObject;
            shadowGameObject.name = sourceGameObject.name + " Shadow";
            ShadowGenerator shadowGenerator = shadowGameObject.GetAddComponent<ShadowGenerator>();
            shadowGenerator.sourceImage = sourceImage;

            RectTransform shadowTransform = shadowGameObject.GetAddComponent<RectTransform>();
            shadowTransform.anchorMin = sourceRectTransform.anchorMin;
            shadowTransform.anchorMax = sourceRectTransform.anchorMax;
            shadowTransform.pivot = sourceRectTransform.pivot;

            bool probablyHasLayout = (sourceGameObject.GetComponent<LayoutGroup>() != null || sourceGameObject.GetComponent<LayoutElement>() != null);

            GameObject newParentGameObject = new GameObject(sourceGameObject.name);
            newParentGameObject.transform.SetParent(sourceRectTransform.parent);
            RectTransform newParentRectTransform = newParentGameObject.GetAddComponent<RectTransform>();
            newParentRectTransform.SetSiblingIndex(sourceRectTransform.GetSiblingIndex());
            newParentRectTransform.anchorMin = sourceRectTransform.anchorMin;
            newParentRectTransform.anchorMax = sourceRectTransform.anchorMax;
            newParentRectTransform.pivot = sourceRectTransform.pivot;
            newParentRectTransform.position = sourcePos;
            newParentRectTransform.sizeDelta = sourceSize;
            LayoutElement layoutElement = null;

            if (probablyHasLayout)
            {
                layoutElement = newParentGameObject.AddComponent<LayoutElement>();
                layoutElement.preferredWidth = sourceLayoutSize.x;
                layoutElement.preferredHeight = sourceLayoutSize.y;
            }

            shadowGameObject.GetComponent<RectTransform>().SetParent(newParentRectTransform, true);
            sourceRectTransform.SetParent(newParentRectTransform, true);

            sourceGameObject.name = sourceGameObject.name + " Image";

            if (probablyHasLayout)
            {
                layoutElement.CalculateLayoutInputHorizontal();
                layoutElement.CalculateLayoutInputVertical();
            }

            shadowGenerator.GenerateShadowFromImage();

            Selection.activeObject = newParentGameObject;
            Undo.RegisterCreatedObjectUndo(shadowGameObject, shadowGameObject.name);
            Undo.RegisterCreatedObjectUndo(newParentGameObject, newParentGameObject.name);
        }

        #endregion
    }
}