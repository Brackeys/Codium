//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Collections.Generic;
using UnityEngine;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/Managers/Ripple Manager")]
    public class RippleManager : MonoBehaviour
    {
        private static RippleManager m_Instance;
        public static RippleManager instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new GameObject("RippleManager", typeof(RippleManager)).GetComponent<RippleManager>();
                }

                return m_Instance;
            }
        }

        private VectorImageData m_RippleImageData;
        public VectorImageData rippleImageData
        {
            get
            {
                if (m_RippleImageData == null)
                {
                    m_RippleImageData = MaterialUIIconHelper.GetIcon("circle").vectorImageData;
                }
                return m_RippleImageData;
            }
        }

        private static int rippleCount;

        private List<Ripple> m_ActiveRipples = new List<Ripple>();
        private Queue<Ripple> m_QueuedRipples = new Queue<Ripple>();

        public Ripple GetRipple()
        {
            if (m_QueuedRipples.Count <= 0)
            {
                CreateRipple();
            }

            Ripple ripple = m_QueuedRipples.Dequeue();
            m_ActiveRipples.Add(ripple);
            ripple.gameObject.SetActive(true);
            return ripple;
        }

        private void CreateRipple()
        {
            Ripple ripple = new GameObject("Ripple " + rippleCount, typeof(VectorImage), typeof(CanvasGroup), typeof(Ripple)).GetComponent<Ripple>();

            ripple.Create(rippleCount, rippleImageData);
            rippleCount++;

            ReleaseRipple(ripple);
        }

        private void ResetRipple(Ripple ripple)
        {
            ripple.rectTransform.SetParent(transform);
            ripple.rectTransform.localScale = Vector3.zero;
            ripple.rectTransform.sizeDelta = Vector2.zero;
            ripple.rectTransform.anchoredPosition = Vector2.zero;
            ripple.image.color = Color.clear;
            ripple.canvasGroup.alpha = 0f;
            ripple.ClearData();
        }

        public void ReleaseRipple(Ripple ripple)
        {
            ResetRipple(ripple);
            ripple.gameObject.SetActive(false);
            m_QueuedRipples.Enqueue(ripple);
        }
    }
}
