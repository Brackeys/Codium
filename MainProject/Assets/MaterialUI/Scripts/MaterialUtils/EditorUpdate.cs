//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MaterialUI
{
    public static class EditorUpdate
    {
		public delegate void EditorUpdateDelegate();
		public static EditorUpdateDelegate onEditorUpdate;

        private static bool m_IsInitialized;

        public static void Init()
        {
            if (!m_IsInitialized)
            {
                EditorApplication.update += Update;
                m_IsInitialized = true;
            }
        }

        public static void Update()
        {
            if (!Application.isPlaying)
            {
                if (onEditorUpdate != null) onEditorUpdate.Invoke();
            }
        }
    }
}
#endif