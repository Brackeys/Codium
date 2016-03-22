//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    [CustomEditor(typeof(MaterialButton), true)]
    [CanEditMultipleObjects]
    public class MaterialButtonEditor : MaterialBaseEditor
    {
        private MaterialButton m_MaterialButton;

		private SerializedProperty m_Interactable;

        private SerializedProperty m_ShadowsCanvasGroup;
        private SerializedProperty m_ContentRectTransform;
        private SerializedProperty m_BackgroundImage;
        private SerializedProperty m_Text;
        private SerializedProperty m_Icon;

        private SerializedProperty m_ContentPaddingX;
        private SerializedProperty m_ContentPaddingY;

        private SerializedProperty m_FitWidthToContent;
        private SerializedProperty m_FitHeightToContent;

        void OnEnable()
        {
            OnBaseEnable();

            m_MaterialButton = (MaterialButton)target;

			m_Interactable = serializedObject.FindProperty("m_Interactable");

            m_ShadowsCanvasGroup = serializedObject.FindProperty("m_ShadowsCanvasGroup");
            m_ContentRectTransform = serializedObject.FindProperty("m_ContentRectTransform");

            m_BackgroundImage = serializedObject.FindProperty("m_BackgroundImage");
            m_Text = serializedObject.FindProperty("m_Text");
            m_Icon = serializedObject.FindProperty("m_Icon");

            m_ContentPaddingX = serializedObject.FindProperty("m_ContentPadding.x");
            m_ContentPaddingY = serializedObject.FindProperty("m_ContentPadding.y");

            m_FitWidthToContent = serializedObject.FindProperty("m_FitWidthToContent");
            m_FitHeightToContent = serializedObject.FindProperty("m_FitHeightToContent");
        }

        void OnDisable()
        {
            OnBaseDisable();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_Interactable);
            if (EditorGUI.EndChangeCheck())
            {
                m_MaterialButton.interactable = m_Interactable.boolValue;
            }

			using (new GUILayout.HorizontalScope())
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(m_FitWidthToContent);
				if (EditorGUI.EndChangeCheck())
				{
					m_MaterialButton.ClearTracker();
				}
				if (m_FitWidthToContent.boolValue)
				{
					EditorGUILayout.LabelField("Padding", GUILayout.Width(52));
					EditorGUILayout.PropertyField(m_ContentPaddingX, new GUIContent());
				}
			}

			using (new GUILayout.HorizontalScope())
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(m_FitHeightToContent);
				if (EditorGUI.EndChangeCheck())
				{
					m_MaterialButton.ClearTracker();
				}
				if (m_FitHeightToContent.boolValue)
				{
					EditorGUILayout.LabelField("Padding", GUILayout.Width(52));
					EditorGUILayout.PropertyField(m_ContentPaddingY, new GUIContent());
				}
			}

            ConvertButtonSection();

            DrawFoldoutExternalProperties(ExternalPropertiesSection);

            DrawFoldoutComponents(ComponentsSection);

            serializedObject.ApplyModifiedProperties();
        }

        private bool ExternalPropertiesSection()
        {
            bool result = false;

            //  Content

            Text text = m_Text.objectReferenceValue as Text;
            if (text != null)
            {
                NonSerializedPropertyField(text, () => text.text = EditorGUILayout.TextField("Button Text", text.text));
                result = true;
            }

            Graphic iconGraphic = null;

            Image image = m_Icon.objectReferenceValue as Image;
            if (image != null)
            {
                NonSerializedPropertyField(image, () => image.sprite = (Sprite)EditorGUILayout.ObjectField(new GUIContent("Button Icon"), image.sprite, typeof(Sprite), true));
                iconGraphic = image;
                result = true;
            }

            VectorImage vectorImage = m_Icon.objectReferenceValue as VectorImage;
            if (vectorImage != null)
            {
                NonSerializedPropertyField(vectorImage, () => MaterialUIInspectorFields.VectorImageDataField("Button Icon", vectorImage.vectorImageData, vectorImage));
                iconGraphic = vectorImage;
                result = true;
            }

            //  Color

            Image backgroundImage = m_BackgroundImage.objectReferenceValue as Image;
            if (backgroundImage != null)
            {
                NonSerializedPropertyField(backgroundImage, () => backgroundImage.color = EditorGUILayout.ColorField("Button Color", backgroundImage.color));
            }

            if (text != null)
            {
                NonSerializedPropertyField(text, () => text.color = EditorGUILayout.ColorField("Text Color", text.color));
            }

            if (iconGraphic != null)
            {
                NonSerializedPropertyField(iconGraphic, () => iconGraphic.color = EditorGUILayout.ColorField("Icon Color", iconGraphic.color));
            }

            return result;
        }

        private void ComponentsSection()
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(m_ContentRectTransform);
            EditorGUILayout.PropertyField(m_BackgroundImage);
            EditorGUILayout.PropertyField(m_ShadowsCanvasGroup);
            EditorGUILayout.PropertyField(m_Text);
            EditorGUILayout.PropertyField(m_Icon);
            EditorGUI.indentLevel--;
        }

        private void ConvertButtonSection()
        {
            GUIContent convertText = new GUIContent();

            if (m_ShadowsCanvasGroup.objectReferenceValue != null)
            {
                convertText.text = "Convert to flat button";
            }
            else
            {
                convertText.text = "Convert to raised button";
            }

            if (Selection.objects.Length > 1)
            {
                GUI.enabled = false;
                convertText.text = "Convert button";
            }

			using (new GUILayout.HorizontalScope())
			{
				GUILayout.Space(EditorGUIUtility.labelWidth);
				if (GUILayout.Button(convertText, EditorStyles.miniButton))
				{
					m_MaterialButton.Convert();
				}
			}

            GUI.enabled = true;
        }
    }
}