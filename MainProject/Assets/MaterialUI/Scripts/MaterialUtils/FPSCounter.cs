//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using UnityEngine.UI;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/FPSCounter", 100)]
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField]
        private float m_UpdateInterval = 0.5f;
		public float updateInterval
		{
			get { return m_UpdateInterval; }
			set { m_UpdateInterval = value; }
		}

		[SerializeField]
		private Text m_Text;
		public Text text
		{
			get { return m_Text; }
			set { m_Text = value; }
		}

        private float m_DeltaFps; // FPS accumulated over the interval
        private int m_Frames; // Frames drawn over the interval
        private float m_Timeleft; // Left time for current interval

        void Start()
        {
            m_Timeleft = updateInterval;
        }

        void Update()
        {
            m_Timeleft -= Time.deltaTime;
            m_DeltaFps += Time.timeScale / Time.deltaTime;
            ++m_Frames;

            // Interval ended - update GUI text and start new interval
            if (m_Timeleft <= 0f)
            {
                // display two fractional digits (f2 format)
                text.text = "" + (m_DeltaFps / m_Frames).ToString("f2") + " FPS";
                if ((m_DeltaFps / m_Frames) < 1)
                {
                    text.text = "";
                }
                m_Timeleft = updateInterval;
                m_DeltaFps = 0f;
                m_Frames = 0;
            }
        }
    }
}