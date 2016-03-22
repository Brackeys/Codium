//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using System.Collections.Generic;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/Managers/Toast Manager")]
    public class ToastManager : MonoBehaviour
    {
        private static ToastManager m_Instance;
        private static ToastManager instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "ToastManager";

                    m_Instance = go.AddComponent<ToastManager>();
                }

                return m_Instance;
            }
        }

        [SerializeField]
        private bool m_KeepBetweenScenes = true;

        [SerializeField]
        private Canvas m_ParentCanvas;

        [Header("Default Toasts parameters")]
        [SerializeField]
        private float m_DefaultDuration = 2f;

        [SerializeField]
        private Color m_DefaultPanelColor = Color.white;

        [SerializeField]
        private Color m_DefaultTextColor = Color.black;

        [SerializeField]
        private int m_DefaultFontSize = 16;

        private Queue<Toast> m_ToastQueue;
        private bool m_IsActive;
        private ToastAnimator m_CurrentAnimator;

        void Awake()
        {
            if (!m_Instance)
            {
                m_Instance = this;

                if (m_KeepBetweenScenes)
                {
                    DontDestroyOnLoad(this);
                }
            }
            else
            {
                Debug.LogWarning("More than one ToastManager exist in the scene, destroying one.");
                Destroy(gameObject);
                return;
            }

            InitSystem();
        }

        private void InitSystem()
        {
            if (m_ParentCanvas == null)
            {
                m_ParentCanvas = FindObjectOfType<Canvas>();
            }

            transform.SetParent(m_ParentCanvas.transform, false);
            transform.localPosition = Vector3.zero;

            m_CurrentAnimator = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.toast, transform).GetComponent<ToastAnimator>();

            m_ToastQueue = new Queue<Toast>();
        }

        void OnDestroy()
        {
            m_Instance = null;
        }

        void OnApplicationQuit()
        {
            m_Instance = null;
        }

        public static void Show(string content)
        {
            Show(content, instance.m_DefaultDuration, instance.m_DefaultPanelColor, instance.m_DefaultTextColor, instance.m_DefaultFontSize);
        }

        public static void Show(string content, float duration, Color panelColor, Color textColor, int fontSize)
        {
            instance.m_ToastQueue.Enqueue(new Toast(content, duration, panelColor, textColor, fontSize));
            instance.StartQueue();
        }

        private void StartQueue()
        {
            if (m_ToastQueue.Count > 0 && !m_IsActive)
            {
                m_CurrentAnimator.Show(m_ToastQueue.Dequeue());
                m_IsActive = true;
            }
        }

        public static bool Remove()
        {
            instance.m_IsActive = false;
            instance.StartQueue();
            return (instance.m_ToastQueue.Count > -1);
        }
    }
}