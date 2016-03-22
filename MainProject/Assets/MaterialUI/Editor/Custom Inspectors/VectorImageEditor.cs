//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEditor;
using UnityEngine;
using System.Linq;

namespace MaterialUI
{
	[CanEditMultipleObjects]
    [CustomEditor(typeof(VectorImage))]
    class VectorImageEditor : Editor
    {
        //  SerializedProperties
        private SerializedProperty m_Size;
        private SerializedProperty m_SizeMode;
        private SerializedProperty m_Color;
	    private SerializedProperty m_VectorImageData;

        void OnEnable()
        {
            m_Size = serializedObject.FindProperty("m_Size");
            m_SizeMode = serializedObject.FindProperty("m_SizeMode");
            m_Color = serializedObject.FindProperty("m_Color");
            m_VectorImageData = serializedObject.FindProperty("m_VectorImageData");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_SizeMode);
            if (m_SizeMode.enumValueIndex == 0)
            {
                EditorGUILayout.PropertyField(m_Size);
            }

            EditorGUILayout.PropertyField(m_Color, new GUIContent("Color"));

			EditorGUILayout.PropertyField(m_VectorImageData, new GUIContent("Icon"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}