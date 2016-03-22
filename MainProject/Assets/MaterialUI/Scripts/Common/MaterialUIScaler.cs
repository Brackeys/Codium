//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;

namespace MaterialUI
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [ExecuteInEditMode]
    [RequireComponent(typeof(Canvas))]
    [AddComponentMenu("MaterialUI/MaterialUI Scaler", 50)]
    public class MaterialUIScaler : UIBehaviour
    {
        public delegate void ScaleFactorChangeEvent(float scaleFactor);
        public ScaleFactorChangeEvent OnScaleFactorChange;

        private Canvas m_TargetCanvas;
        public Canvas targetCanvas
        {
            get
            {
                if (m_TargetCanvas == null)
                {
                    m_TargetCanvas = GetComponent<Canvas>();
                }
                return m_TargetCanvas;
            }
        }

        [HideInInspector]
        [SerializeField]
        private float m_LastScaleFactor;
        public float scaleFactor
        {
            get { return m_LastScaleFactor; }
        }

#if UNITY_EDITOR
        public MaterialUIScaler()
        {
            EditorUpdate.Init();
            EditorUpdate.onEditorUpdate += CheckScaleFactor;
        }

        protected override void OnDestroy()
        {
            EditorUpdate.onEditorUpdate -= CheckScaleFactor;
        }
#endif

        void Update()
        {
            if (Application.isPlaying)
            {
                CheckScaleFactor();
            }
        }

        private void CheckScaleFactor()
        {
#if UNITY_EDITOR
            if (IsDestroyed())
            {
                EditorUpdate.onEditorUpdate -= CheckScaleFactor;
                return;
            }
#endif

            if (targetCanvas == null) return;

            if (m_LastScaleFactor != targetCanvas.scaleFactor)
            {
                m_LastScaleFactor = targetCanvas.scaleFactor;
                if (OnScaleFactorChange != null)
                {
                    OnScaleFactorChange(m_LastScaleFactor);
                }
            }
        }

        public static MaterialUIScaler GetParentScaler(Transform transform)
        {
            if (transform == null) return null;

            Transform currentTransform = transform;
            MaterialUIScaler scaler = null;

            while (currentTransform.root != currentTransform)
            {
                currentTransform = currentTransform.parent;
                scaler = currentTransform.GetComponent<MaterialUIScaler>();
                if (scaler != null) break;
            }

            return scaler;
        }
    }
}