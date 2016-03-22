//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using System;
using System.Collections.Generic;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/Managers/Snackbar Manager")]
    public class SnackbarManager : MonoBehaviour
    {
        private static SnackbarManager m_Instance;
        private static SnackbarManager instance
        {
            get
            {
                if (m_Instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = "SnackbarManager";
                    go.AddComponent<CanvasRenderer>();
                    RectTransform rectTransform = go.AddComponent<RectTransform>();
                    rectTransform.anchoredPosition = Vector2.zero;
                    rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                    m_Instance = go.AddComponent<SnackbarManager>();
                }

                return m_Instance;
            }
        }

        [SerializeField]
        private bool m_KeepBetweenScenes = true;

        [SerializeField]
        private Canvas m_ParentCanvas;

        [Header("Default Snackbar parameters")]
        [SerializeField]
        private float m_DefaultDuration = 5f;

        [SerializeField]
        private Color m_DefaultPanelColor = MaterialColor.HexToColor("323232");

        [SerializeField]
        private Color m_DefaultTextColor = MaterialColor.textLight;

        [SerializeField]
        private int m_DefaultFontSize = 16;

        private Queue<Snackbar> m_Snackbars;
        private bool m_IsActive;
        private SnackbarAnimator m_CurrentAnimator;

        private bool m_InitDone = false;

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
                Debug.LogWarning("More than one SnackbarManager exist in the scene, destroying one.");
                Destroy(gameObject);
            }
        }

        private void InitSystem()
        {
            if (m_ParentCanvas == null)
            {
                m_ParentCanvas = FindObjectOfType<Canvas>();
            }

            transform.SetParent(m_ParentCanvas.transform, false);
            transform.localPosition = Vector3.zero;
            
            m_CurrentAnimator = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.snackbar, transform).GetComponent<SnackbarAnimator>();

            m_Snackbars = new Queue<Snackbar>();

            m_InitDone = true;
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
            Show(content, "Okay", null);
        }

        public static void Show(string content, string actionName, Action onActionButtonClicked)
        {
            Show(content, instance.m_DefaultDuration, instance.m_DefaultPanelColor, instance.m_DefaultTextColor, instance.m_DefaultFontSize, actionName, onActionButtonClicked);
        }

        public static void Show(string content, float duration, Color panelColor, Color textColor, int fontSize, string actionName, Action onActionButtonClicked)
        {
            if (!instance.m_InitDone)
            {
                instance.InitSystem();
            }

            instance.m_Snackbars.Enqueue(new Snackbar(content, duration, panelColor, textColor, fontSize, actionName, onActionButtonClicked));
            instance.StartQueue();
        }

        private void StartQueue()
        {
            if (m_Snackbars.Count > 0 && !m_IsActive)
            {
                m_CurrentAnimator.Show(m_Snackbars.Dequeue());
                m_IsActive = true;
            }
        }

        public static bool Remove()
        {
            instance.m_IsActive = false;
            instance.StartQueue();
            return (instance.m_Snackbars.Count > -1);
        }
    }
}