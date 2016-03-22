//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System.Collections.Generic;
using UnityEngine;

namespace MaterialUI
{
    public static class PrefabManager
    {
        private static readonly List<GameObject> m_Prefabs = new List<GameObject>();
        private static readonly List<string> m_Names = new List<string>();
        
        public static class ResourcePrefabs
        {
            public const string progressIndicatorCircular = "Progress Indicators/Circle Progress Indicator";
            public const string progressIndicatorLinear = "Progress Indicators/Linear Progress Indicator";

            public const string dialogAlert = "Dialogs/DialogAlert";
            public const string dialogProgress = "Dialogs/DialogProgress";
            public const string dialogSimpleList = "Dialogs/DialogSimpleList";
            public const string dialogCheckboxList = "Dialogs/DialogCheckboxList";
            public const string dialogRadioList = "Dialogs/DialogRadioList";
            public const string dialogTimePicker = "Dialogs/Pickers/DialogTimePicker";
            public const string dialogDatePicker = "Dialogs/Pickers/DialogDatePicker";

            public const string disabledPanel = "DisabledPanel";
            public const string sliderDot = "SliderDot";
            public const string dropdownPanel = "Menus/Dropdown Panel";

            public const string snackbar = "Snackbar";
            public const string toast = "Toast";
        }

        public static GameObject GetGameObject(string nameWithPath)
        {
            GameObject gameObject = null;

            if (!m_Names.Contains(nameWithPath))
            {
                gameObject = Resources.Load<GameObject>(nameWithPath);

                if (gameObject != null)
                {
                    m_Prefabs.Add(gameObject);
                    m_Names.Add(nameWithPath);
                }
            }
            else
            {
                for (int i = 0; i < m_Prefabs.Count; i++)
                {
                    if (m_Names[i] == nameWithPath)
                    {
                        if (m_Prefabs[i] != null)
                        {
                            gameObject = m_Prefabs[i];
                        }
                    }
                }
            }

            return gameObject;
        }

        public static GameObject InstantiateGameObject(string nameWithPath, Transform parent)
        {
            GameObject go = GetGameObject(nameWithPath);

            if (go == null)
            {
                return null;
            }

            go = GameObject.Instantiate(go);

            if (parent == null)
            {
                return go;
            }

            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
            go.transform.localEulerAngles = Vector3.zero;
            go.transform.localPosition = Vector3.zero;

            return go;
        }
    }
}