//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEditor;
using UnityEngine;

namespace MaterialUI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MaterialRipple))]
    class MaterialRippleEditor : Editor
    {
        private SerializedProperty m_RippleData;
        private SerializedProperty m_ToggleMask;
        private SerializedProperty m_HighlightWhen;
        private SerializedProperty m_HighlightGraphic;
        private SerializedProperty m_AutoHighlightColor;
        private SerializedProperty m_HighlightColor;
        private SerializedProperty m_CheckForScroll;
        private SerializedProperty m_AutoHighlightBlendAmount;

        void OnEnable()
        {
            m_RippleData = serializedObject.FindProperty("m_RippleData");
            m_ToggleMask = serializedObject.FindProperty("m_ToggleMask");
            m_HighlightWhen = serializedObject.FindProperty("m_HighlightWhen");
            m_HighlightGraphic = serializedObject.FindProperty("m_HighlightGraphic");
            m_AutoHighlightColor = serializedObject.FindProperty("m_AutoHighlightColor");
            m_HighlightColor = serializedObject.FindProperty("m_HighlightColor");
            m_CheckForScroll = serializedObject.FindProperty("m_CheckForScroll");
            m_AutoHighlightBlendAmount = serializedObject.FindProperty("m_AutoHighlightBlendAmount");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            {
                EditorGUILayout.PropertyField(m_RippleData, true);
                serializedObject.ApplyModifiedProperties();
            }
            if (EditorGUI.EndChangeCheck())
            {
                ((MaterialRipple)serializedObject.targetObject).RefreshSettings();
            }

            EditorGUILayout.PropertyField(m_ToggleMask, true);

            EditorGUILayout.PropertyField(m_HighlightWhen, true);

            if (m_HighlightWhen.intValue > 0)
            {
                EditorGUILayout.PropertyField(m_HighlightGraphic, true);

                EditorGUI.BeginChangeCheck();
                {
                    EditorGUILayout.PropertyField(m_AutoHighlightColor, true);
                    if (m_AutoHighlightColor.boolValue)
                    {
                        EditorGUILayout.PropertyField(m_AutoHighlightBlendAmount, new GUIContent("Blend Amount"), true);
                    }
                    serializedObject.ApplyModifiedProperties();
                }
                if (EditorGUI.EndChangeCheck())
                {
                    m_AutoHighlightBlendAmount.floatValue = Mathf.Clamp(m_AutoHighlightBlendAmount.floatValue, 0f, 1f);
                    ((MaterialRipple)serializedObject.targetObject).RefreshSettings();
                }

                if (m_AutoHighlightColor.boolValue == false)
                {
                    EditorGUILayout.PropertyField(m_HighlightColor, true);
                }
            }

            EditorGUILayout.PropertyField(m_CheckForScroll, true);

            serializedObject.ApplyModifiedProperties();
        }
    }
}