//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace MaterialUI
{
    public static class ContinuationManager
    {
        private class ContinuationJob
        {
            public ContinuationJob(Func<bool> completed, Action continueWith)
            {
                Completed = completed;
                ContinueWith = continueWith;
            }
            public Func<bool> Completed { get; private set; }
            public Action ContinueWith { get; private set; }
        }

        private static readonly List<ContinuationJob> m_Jobs = new List<ContinuationJob>();

        public static void Add(Func<bool> completed, Action continueWith)
        {
            if (!m_Jobs.Any())
            {
                EditorApplication.update += Update;
            }

            m_Jobs.Add(new ContinuationJob(completed, continueWith));
        }

        private static void Update()
        {
            for (int i = 0; i >= 0; --i)
            {
                var jobIt = m_Jobs[i];
                if (jobIt.Completed())
                {
                    jobIt.ContinueWith();
                    m_Jobs.RemoveAt(i);
                }
            }
            if (!m_Jobs.Any())
            {
                EditorApplication.update -= Update;
            }
        }
    }
}
#endif