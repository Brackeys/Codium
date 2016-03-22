//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEditor;
using UnityEditor.AnimatedValues;
using System;

namespace MaterialUI
{
    public class MaterialBaseEditor : Editor
    {
        private bool m_ShowColors;
        private bool m_ShowComponents;
        private bool m_ShowExternalProperties;

        private string m_ColorsPrefKey;
        private string m_ComponentsPrefKey;
        private string m_ExternalPropertiesPrefKey;

        private AnimBool m_ShowColorsAnimBool;
        private AnimBool m_ShowComponentsAnimBool;
        private AnimBool m_ShowExternalPropertiesAnimBool;

        protected void OnBaseEnable()
        {
            string prefKey = GetType().Name;

            m_ColorsPrefKey = prefKey + "_Show_Colors";
            m_ComponentsPrefKey = prefKey + "_Show_Components";
            m_ExternalPropertiesPrefKey = prefKey + "_Show_External_Properties";

            m_ShowColors = EditorPrefs.GetBool(m_ColorsPrefKey, true);
            m_ShowComponents = EditorPrefs.GetBool(m_ComponentsPrefKey, false);
            m_ShowExternalProperties = EditorPrefs.GetBool(m_ExternalPropertiesPrefKey, true);

            m_ShowColorsAnimBool = new AnimBool { value = m_ShowColors };
            m_ShowColorsAnimBool.valueChanged.AddListener(Repaint);
            m_ShowComponentsAnimBool = new AnimBool { value = m_ShowComponents };
            m_ShowComponentsAnimBool.valueChanged.AddListener(Repaint);
            m_ShowExternalPropertiesAnimBool = new AnimBool { value = m_ShowExternalProperties };
            m_ShowExternalPropertiesAnimBool.valueChanged.AddListener(Repaint);
        }

        protected void OnBaseDisable()
        {
            if (m_ShowComponentsAnimBool != null)
            {
                m_ShowComponentsAnimBool.valueChanged.RemoveListener(Repaint);
            }
            if (m_ShowColorsAnimBool != null)
            {
                m_ShowColorsAnimBool.valueChanged.RemoveListener(Repaint);
            }
        }

        protected void DrawFoldoutColors(Action drawSection)
        {
            EditorGUI.BeginChangeCheck();
            m_ShowColors = EditorGUILayout.Foldout(m_ShowColors, "Colors");
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(m_ColorsPrefKey, m_ShowColors);
            }

            m_ShowColorsAnimBool.target = m_ShowColors;

            if (EditorGUILayout.BeginFadeGroup(m_ShowColorsAnimBool.faded))
            {
                drawSection();
            }
            EditorGUILayout.EndFadeGroup();
        }

        protected void DrawFoldoutComponents(Action drawSection)
        {
            EditorGUI.BeginChangeCheck();
            m_ShowComponents = EditorGUILayout.Foldout(m_ShowComponents, "Components");
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(m_ComponentsPrefKey, m_ShowComponents);
            }

            m_ShowComponentsAnimBool.target = m_ShowComponents;

            if (EditorGUILayout.BeginFadeGroup(m_ShowComponentsAnimBool.faded))
            {
                drawSection();
            }
            EditorGUILayout.EndFadeGroup();
        }



        protected void DrawFoldoutExternalProperties(Func<bool> drawSection)
        {
            EditorGUI.BeginChangeCheck();
            m_ShowExternalProperties = EditorGUILayout.Foldout(m_ShowExternalProperties, "External Properties");
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(m_ExternalPropertiesPrefKey, m_ShowExternalProperties);
            }

            m_ShowExternalPropertiesAnimBool.target = m_ShowExternalProperties;

            if (EditorGUILayout.BeginFadeGroup(m_ShowExternalPropertiesAnimBool.faded))
            {
                if (!drawSection.Invoke())
                {
                    EditorGUILayout.LabelField("No External Properties Editable");
                }
            }
            EditorGUILayout.EndFadeGroup();
        }

        protected void NonSerializedPropertyField<T>(T objectToDo, Action fieldAction, Action onChangeAction = null, Action onNoChangeAction = null) where T : UnityEngine.Object
        {
            Undo.RecordObject(objectToDo, "edit button text");
            EditorGUI.BeginChangeCheck();
            {
                fieldAction.InvokeIfNotNull();
            }
            if (EditorGUI.EndChangeCheck())
            {
                onChangeAction.InvokeIfNotNull();
                EditorUtility.SetDirty(objectToDo);
            }
            else
            {
                Undo.ClearUndo(objectToDo);
                onNoChangeAction.InvokeIfNotNull();
            }
        }
    }
}