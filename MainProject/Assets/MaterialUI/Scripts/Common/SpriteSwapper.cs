//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MaterialUI
{
    [ExecuteInEditMode]
    [AddComponentMenu("MaterialUI/Sprite Swapper", 50)]
    public class SpriteSwapper : UIBehaviour
    {
        [SerializeField]
        private Image m_TargetImage;
		public Image targetImage
		{
			get { return m_TargetImage; }
			set
			{
				m_TargetImage = value;
				RefreshSprite();
			}
		}

        [SerializeField]
        private MaterialUIScaler m_RootScaler;
		public MaterialUIScaler rootScaler
		{
			get
			{
				if (m_RootScaler == null)
				{
					m_RootScaler = MaterialUIScaler.GetParentScaler(transform);
				}
				return m_RootScaler;
			}
		}

        [SerializeField]
        private Sprite m_Sprite1X;
		public Sprite sprite1X
		{
			get { return m_Sprite1X; }
			set
			{
				m_Sprite1X = value;
				RefreshSprite();
			}
		}

        [SerializeField]
        private Sprite m_Sprite2X;
		public Sprite sprite2X
		{
			get { return m_Sprite2X; }
			set
			{
				m_Sprite2X = value;
				RefreshSprite();
			}
		}

        [SerializeField]
        private Sprite m_Sprite4X;
		public Sprite sprite4X
		{
			get { return m_Sprite4X; }
			set
			{
				m_Sprite4X = value;
				RefreshSprite();
			}
		}

        private Sprite m_LastSprite1X;
        private Sprite m_LastSprite2X;
        private Sprite m_LastSprite4X;

        protected override void OnEnable()
        {
            if (!targetImage)
            {
                targetImage = gameObject.GetComponent<Image>();
            }

            RefreshSprite();
        }

        protected override void Start()
        {
            if (rootScaler == null) return;
            rootScaler.OnScaleFactorChange += SwapSprite;
            RefreshSprite();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            RefreshSprite();
        }
#endif

        public void RefreshSprite()
        {
            if (rootScaler == null) return;
            SwapSprite(rootScaler.scaleFactor);
        }

        private void SwapSprite(float scaleFactor)
        {
            if (!targetImage) return;

            if (scaleFactor > 2f && sprite4X)
            {
                targetImage.sprite = sprite4X;
            }
            else if (scaleFactor > 1f && sprite2X)
            {
                targetImage.sprite = sprite2X;
            }
            else
            {
                targetImage.sprite = sprite1X;
            }
        }
    }
}