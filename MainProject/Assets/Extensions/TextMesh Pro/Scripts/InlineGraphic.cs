﻿// Copyright (C) 2014 - 2015 Stephan Bouchard - All Rights Reserved
// This code can only be used under the standard Unity Asset Store End User License Agreement
// A Copy of the EULA APPENDIX 1 is available at http://unity3d.com/company/legal/as_terms

#if UNITY_4_6 || UNITY_5


using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;



namespace TMPro
{

    public class InlineGraphic : MaskableGraphic
    {

        public Texture texture;

        public override Texture mainTexture
        {
            get
            {
                if ((Object)this.texture == (Object)null)
                    return (Texture)Graphic.s_WhiteTexture;
                else
                    return this.texture;
            }
        }


        private InlineGraphicManager m_manager;
        private RectTransform m_RectTransform;
        private RectTransform m_ParentRectTransform;
        //private CanvasRenderer m_canvasRenderer;

        //private List<UIVertex> m_uiVertices;

        protected override void Awake()
        {
            //base.Awake();

            m_manager = GetComponentInParent<InlineGraphicManager>();
        }


        protected override void OnEnable()
        {
            //GraphicRegistry.UnregisterGraphicForCanvas(this.canvas, this);
            //base.OnEnable();

            if (m_RectTransform == null) m_RectTransform = gameObject.GetComponent<RectTransform>();

            if (m_manager != null && m_manager.spriteAsset != null)
                texture = m_manager.spriteAsset.spriteSheet;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            //Debug.Log("Texture ID is " + this.texture.GetInstanceID());
        }
#endif


        //protected override void OnBeforeTransformParentChanged()
        //{
        //    //GraphicRegistry.UnregisterGraphicForCanvas(this.canvas, this);
        //}


        protected override void OnTransformParentChanged()
        {
            //Debug.Log("OnTransformParentChanged()");
            //base.OnTransformParentChanged();
        }

        protected override void OnRectTransformDimensionsChange()
        {
            if (m_RectTransform == null) m_RectTransform = gameObject.GetComponent<RectTransform>();
            if (m_ParentRectTransform == null) m_ParentRectTransform = m_RectTransform.parent.GetComponent<RectTransform>();

            // RectTransform properties of the parent and child have to remain in sync for proper alignment.
            if (m_RectTransform.pivot != m_ParentRectTransform.pivot)
                m_RectTransform.pivot = m_ParentRectTransform.pivot;
        }


        //protected override void OnCanvasHierarchyChanged()
        //{
        //    Debug.Log("OnCanvasHierarchyChanged()");

        //    //base.OnCanvasHierarchyChanged();
        //}


        //public override void SetLayoutDirty()
        //{
        //    //base.SetLayoutDirty();
        //}


        //public override void SetVerticesDirty()
        //{
        //    //base.SetVerticesDirty();
        //}


        //public override void SetMaterialDirty()
        //{
        //    //base.SetMaterialDirty();
        //}


        public new void UpdateMaterial()
        {
            base.UpdateMaterial();
        }

        
        protected override void UpdateGeometry()
        {
            // This function needs to be override otherwise Unity alters the content of the geometry.
            //Debug.Log("UpdateGeometry called.");
        }

    }
}

#endif