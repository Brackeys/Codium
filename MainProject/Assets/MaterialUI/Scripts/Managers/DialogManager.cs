//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;
using UnityEngine;

namespace MaterialUI
{
    [AddComponentMenu("MaterialUI/Managers/Dialog Manager")]
    public class DialogManager : MonoBehaviour
    {
        private static DialogManager m_Instance;
        private static DialogManager instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new GameObject("DialogManager").AddComponent<DialogManager>();
                    m_Instance.InitDialogSystem();
                }

                return m_Instance;
            }
        }

        [SerializeField]
        private Canvas m_ParentCanvas;

        private RectTransform m_RectTransform;
        public static RectTransform rectTransform
        {
            get
            {
                if (instance.m_RectTransform == null)
                {
                    instance.m_RectTransform = instance.transform as RectTransform;
                }

                return instance.m_RectTransform;
            }
        }

        void Awake()
        {
            if (!m_Instance)
            {
                m_Instance = this;
                m_Instance.InitDialogSystem();
            }
            else
            {
                Debug.LogWarning("More than one DialogManager exist in the scene, destroying one.");
                Destroy(gameObject);
            }
        }

        void OnDestroy()
        {
            m_Instance = null;
        }

        void OnApplicationQuit()
        {
            m_Instance = null;
        }

        private void InitDialogSystem()
        {
            m_RectTransform = gameObject.GetAddComponent<RectTransform>();

            if (m_ParentCanvas == null)
            {
                transform.SetParent(FindObjectOfType<Canvas>().transform, true);
                m_ParentCanvas = rectTransform.root.GetComponent<Canvas>();
            }

            transform.SetParent(m_ParentCanvas.transform, false);
            transform.localScale = Vector3.one;

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.localPosition = Vector2.zero;
        }

        public static DialogAlert ShowAlert(string bodyText, string titleText, ImageData icon)
        {
            return ShowAlert(bodyText, null, "OK", titleText, icon);
        }

        public static DialogAlert ShowAlert(string bodyText, Action onAffirmativeButtonClicked, string affirmativeButtonText, string titleText, ImageData icon)
        {
            return ShowAlert(bodyText, onAffirmativeButtonClicked, affirmativeButtonText, titleText, icon, null, null);
        }

        public static DialogAlert ShowAlert(string bodyText, Action onAffirmativeButtonClicked, string affirmativeButtonText, string titleText, ImageData icon, Action onDismissiveButtonClicked, string dismissiveButtonText)
        {
            DialogAlert dialog = CreateAlert();
            dialog.Initialize(bodyText, onAffirmativeButtonClicked, affirmativeButtonText, titleText, icon, onDismissiveButtonClicked, dismissiveButtonText);
            dialog.Show();
            return dialog;
        }

        public static DialogAlert CreateAlert()
        {
            DialogAlert dialog = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.dialogAlert, instance.transform).GetComponent<DialogAlert>();
            dialog.Initialize();
            return dialog;
        }

        public static DialogProgress ShowProgressLinear(string bodyText)
        {
            return ShowProgressLinear(bodyText, null, null, false);
        }

        public static DialogProgress ShowProgressLinear(string bodyText, string titleText, ImageData icon, bool startStationaryAtZero = false)
        {
            DialogProgress dialog = CreateProgressLinear();
            dialog.Initialize(bodyText, titleText, icon, startStationaryAtZero);
            dialog.ShowModal();
            return dialog;
        }

        public static DialogProgress CreateProgressLinear()
        {
            DialogProgress dialog = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.dialogProgress, instance.transform).GetComponent<DialogProgress>();
            dialog.SetupIndicator(true);
            dialog.Initialize();
            return dialog;
        }

        public static DialogProgress ShowProgressCircular(string bodyText)
        {
            return ShowProgressCircular(bodyText, null, null, false);
        }

        public static DialogProgress ShowProgressCircular(string bodyText, string titleText, ImageData icon, bool startStationaryAtZero = false)
        {
            DialogProgress dialog = CreateProgressCircular();
            dialog.Initialize(bodyText, titleText, icon, startStationaryAtZero);
            dialog.ShowModal();
            return dialog;
        }

        public static DialogProgress CreateProgressCircular()
        {
            DialogProgress dialog = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.dialogProgress, instance.transform).GetComponent<DialogProgress>();
            dialog.SetupIndicator(false);
            dialog.Initialize();
            return dialog;
        }

        public static DialogSimpleList ShowSimpleList(string[] options, Action<int> onItemClick)
        {
            return ShowSimpleList(options, onItemClick, null, null);
        }

        public static DialogSimpleList ShowSimpleList(string[] options, Action<int> onItemClick, string titleText, ImageData icon)
        {
            OptionDataList optionDataList = new OptionDataList();

            for (int i = 0; i < options.Length; i++)
            {
                OptionData optionData = new OptionData(options[i], null);
                optionDataList.options.Add(optionData);
            }

            return DialogManager.ShowSimpleList(optionDataList, onItemClick, titleText, icon);
        }

        public static DialogSimpleList ShowSimpleList(OptionDataList optionDataList, Action<int> onItemClick)
        {
            return ShowSimpleList(optionDataList, onItemClick, null, null);
        }

        public static DialogSimpleList ShowSimpleList(OptionDataList optionDataList, Action<int> onItemClick, string titleText, ImageData icon)
        {
            DialogSimpleList dialog = CreateSimpleList();
            dialog.Initialize(optionDataList, onItemClick, titleText, icon);
            dialog.Show();
            return dialog;
        }

        public static DialogSimpleList CreateSimpleList()
        {
            DialogSimpleList dialog = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.dialogSimpleList, instance.transform).GetComponent<DialogSimpleList>();
            dialog.Initialize();
            return dialog;
        }

        public static DialogCheckboxList ShowCheckboxList(string[] options, Action<bool[]> onAffirmativeButtonClicked, string affirmativeButtonText = "OK")
        {
            return ShowCheckboxList(options, onAffirmativeButtonClicked, affirmativeButtonText, null, null);
        }

        public static DialogCheckboxList ShowCheckboxList(string[] options, Action<bool[]> onAffirmativeButtonClicked, string affirmativeButtonText, string titleText, ImageData icon)
        {
            return ShowCheckboxList(options, onAffirmativeButtonClicked, affirmativeButtonText, titleText, icon, null, null);
        }

        public static DialogCheckboxList ShowCheckboxList(string[] options, Action<bool[]> onAffirmativeButtonClicked, string affirmativeButtonText, string titleText, ImageData icon, Action onDismissiveButtonClicked, string dismissiveButtonText)
        {
            DialogCheckboxList dialog = CreateCheckboxList();
            dialog.Initialize(options, onAffirmativeButtonClicked, affirmativeButtonText, titleText, icon, onDismissiveButtonClicked, dismissiveButtonText);
            dialog.Show();
            return dialog;
        }

        public static DialogCheckboxList CreateCheckboxList()
        {
            DialogCheckboxList dialog = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.dialogCheckboxList, instance.transform).GetComponent<DialogCheckboxList>();
            dialog.Initialize();
            return dialog;
        }

        public static DialogRadioList ShowRadioList(string[] options, Action<int> onAffirmativeButtonClicked, string affirmativeButtonText = "OK")
        {
            return ShowRadioList(options, onAffirmativeButtonClicked, affirmativeButtonText, 0);
        }

        public static DialogRadioList ShowRadioList(string[] options, Action<int> onAffirmativeButtonClicked, string affirmativeButtonText, int selectedIndexStart)
        {
            return ShowRadioList(options, onAffirmativeButtonClicked, affirmativeButtonText, null, null, selectedIndexStart);
        }

        public static DialogRadioList ShowRadioList(string[] options, Action<int> onAffirmativeButtonClicked, string affirmativeButtonText, string titleText, ImageData icon)
        {
            return ShowRadioList(options, onAffirmativeButtonClicked, affirmativeButtonText, titleText, icon, null, null, 0);
        }

        public static DialogRadioList ShowRadioList(string[] options, Action<int> onAffirmativeButtonClicked, string affirmativeButtonText, string titleText, ImageData icon, int selectedIndexStart)
        {
            return ShowRadioList(options, onAffirmativeButtonClicked, affirmativeButtonText, titleText, icon, null, null, selectedIndexStart);
        }

        public static DialogRadioList ShowRadioList(string[] options, Action<int> onAffirmativeButtonClicked, string affirmativeButtonText, string titleText, ImageData icon, Action onDismissiveButtonClicked, string dismissiveButtonText, int selectedIndexStart = 0)
        {
            DialogRadioList dialog = CreateRadioList();
            dialog.Initialize(options, onAffirmativeButtonClicked, affirmativeButtonText, titleText, icon, onDismissiveButtonClicked, dismissiveButtonText, selectedIndexStart);
            dialog.Show();
            return dialog;
        }

        public static DialogRadioList CreateRadioList()
        {
            DialogRadioList dialog = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.dialogRadioList, instance.transform).GetComponent<DialogRadioList>();
            dialog.Initialize();
            return dialog;
        }

        public static T CreateCustomDialog<T>(string dialogPrefabPath) where T : MaterialDialog
        {
            T dialog = PrefabManager.InstantiateGameObject(dialogPrefabPath, instance.transform).GetComponent<T>();
            return dialog;
        }

        public static void ShowTimePicker(int hour, int minute, bool isAM, Action<int, int, bool> onAffirmativeClicked)
        {
            ShowTimePicker(hour, minute, isAM, onAffirmativeClicked, MaterialColor.teal500);
        }

        public static void ShowTimePicker(int hour, int minute, bool isAM, Action<int, int, bool> onAffirmativeClicked, Color accentColor)
        {
            DialogTimePicker dialog = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.dialogTimePicker, instance.transform).GetComponent<DialogTimePicker>();
            dialog.Initialize(hour, minute, isAM, onAffirmativeClicked, accentColor);
            dialog.Show();
        }

        public static void ShowDatePicker(int year, int month, int day, Action<DateTime> onAffirmativeClicked)
        {
            ShowDatePicker(year, month, day, onAffirmativeClicked, MaterialColor.teal500);
        }

        public static void ShowDatePicker(int year, int month, int day, Action<DateTime> onAffirmativeClicked, Color accentColor)
        {
            DialogDatePicker dialog = PrefabManager.InstantiateGameObject(PrefabManager.ResourcePrefabs.dialogDatePicker, instance.transform).GetComponent<DialogDatePicker>();
            dialog.Initialize(year, month, day, onAffirmativeClicked, accentColor);
            dialog.Show();
        }
    }
}