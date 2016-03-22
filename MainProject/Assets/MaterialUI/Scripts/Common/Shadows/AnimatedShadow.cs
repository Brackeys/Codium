//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/Shadow Anim", 100)]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(Image))]
    public class AnimatedShadow : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        private bool m_IsVisible;
        public bool isVisible
        {
            get { return m_IsVisible; }
            set { m_IsVisible = value; }
        }

        private CanvasGroup m_CanvasGroup;
        public CanvasGroup canvasGroup
        {
            get
            {
                if (m_CanvasGroup == null)
                {
                    m_CanvasGroup = GetComponent<CanvasGroup>();
                }
                return m_CanvasGroup;
            }
        }

        private int m_Tweener;

        public void SetShadow(bool set, bool instant = false)
        {
            isVisible = set;

            if (set)
            {
                gameObject.SetActive(true);
            }

            if (Application.isPlaying && !instant)
            {
                TweenManager.EndTween(m_Tweener);

                m_Tweener = TweenManager.TweenFloat(f => canvasGroup.alpha = f, () => canvasGroup.alpha, set ? 1f : 0f, 0.5f, 0f, () => gameObject.SetActive(set));
            }
            else
            {
                canvasGroup.alpha = set ? 1f : 0f;
                gameObject.SetActive(set);
            }
        }
    }
}
