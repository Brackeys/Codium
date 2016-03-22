//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaterialUI
{
    [ExecuteInEditMode]
    public class TweenManager : MonoBehaviour
    {
        #region shared

        [Serializable]
        private class TweenQueue<T> where T : AutoTween, new()
        {
            [SerializeField]
            private Queue<T> m_Tweens = new Queue<T>();
            [SerializeField]
            public Queue<T> tweens
            {
                get { return m_Tweens; }
            }

            [SerializeField]
            public T GetTween()
            {
                if (m_Tweens.Count > 0)
                {
                    return m_Tweens.Dequeue();
                }
                else
                {
                    return new T();
                }
            }
        }

        [SerializeField]
        private static TweenManager m_Instance;

        private static TweenManager instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new GameObject("Tween Manager").AddComponent<TweenManager>();
                }
                return m_Instance;
            }
        }

        public int totalTweenCount
        {
            get { return activeTweenCount + dormantTweenCount; }
        }

        public int activeTweenCount
        {
            get { return m_ActiveTweens.Count; }
        }

        public int dormantTweenCount
        {
            get
            {
                int count = 0;
                count += m_TweenIntQueue.tweens.Count;
                count += m_TweenFloatQueue.tweens.Count;
                count += m_TweenVector2Queue.tweens.Count;
                count += m_TweenVector3Queue.tweens.Count;
                count += m_TweenVector4Queue.tweens.Count;
                count += m_TweenColorQueue.tweens.Count;
                return count;
            }
        }

        private bool m_ReadyToKill;

        public void OnApplicationQuit()
        {
            m_ReadyToKill = true;
        }

        [SerializeField]
        private List<AutoTween> m_ActiveTweens = new List<AutoTween>();

        private int m_TweenIdCount = 1;

        private bool m_FirstFrame = true;

#if UNITY_EDITOR
        private void Start()
        {
            if (!Application.isPlaying)
            {
                DestroyImmediate(gameObject);
            }
        }
#endif

        private void Update()
        {
            if (m_FirstFrame)
            {
                m_FirstFrame = false;
                return;
            }

            for (int i = 0; i < m_ActiveTweens.Count; i++)
            {
                m_ActiveTweens[i].UpdateTween();
            }

            if (m_ReadyToKill)
            {
                Destroy(gameObject);
            }

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                DestroyImmediate(gameObject);
            }
#endif
        }

        public static void Release(AutoTween tween)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            instance.m_ActiveTweens.Remove(tween);

            if (tween.GetType() == typeof(AutoTweenFloat))
            {
                instance.m_TweenFloatQueue.tweens.Enqueue((AutoTweenFloat)tween);
            }
        }

        public static bool TweenIsActive(int id)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return false;
#endif
            for (int i = 0; i < instance.m_ActiveTweens.Count; i++)
            {
                if (instance.m_ActiveTweens[i].tweenId == id)
                {
                    return true;
                }
            }

            return false;
        }

        public static void EndTween(int id, bool callCallback = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying) return;
#endif
            for (int i = 0; i < instance.m_ActiveTweens.Count; i++)
            {
                AutoTween tween = instance.m_ActiveTweens[i];

                if (tween.tweenId == id)
                {
                    tween.EndTween(callCallback);
                }
            }
        }

        #endregion

        #region generic

        public static int TweenValue<T>(Action<T> updateValue, T startValue, T targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenValue<T>(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime, tweenType);
        }

        public static int TweenValue<T>(Action<T> updateValue, Func<T> startValue, T targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenValue<T>(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime, tweenType);
        }

        public static int TweenValue<T>(Action<T> updateValue, Func<T> startValue, Func<T> targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            if (typeof(T) == typeof(float))
            {
                return TweenFloat(updateValue as Action<float>, startValue as Func<float>, targetValue as Func<float>, duration, delay, callback, scaledTime, tweenType);
            }
            else if (typeof(T) == typeof(int))
            {
                return TweenInt(updateValue as Action<int>, startValue as Func<int>, targetValue as Func<int>, duration, delay, callback, scaledTime, tweenType);
            }
            else if (typeof(T) == typeof(Vector2))
            {
                return TweenVector2(updateValue as Action<Vector2>, startValue as Func<Vector2>, targetValue as Func<Vector2>, duration, delay, callback, scaledTime, tweenType);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                return TweenVector3(updateValue as Action<Vector3>, startValue as Func<Vector3>, targetValue as Func<Vector3>, duration, delay, callback, scaledTime, tweenType);
            }
            else if (typeof(T) == typeof(Vector4))
            {
                return TweenVector4(updateValue as Action<Vector4>, startValue as Func<Vector4>, targetValue as Func<Vector4>, duration, delay, callback, scaledTime, tweenType);
            }
            else if (typeof(T) == typeof(Color))
            {
                return TweenColor(updateValue as Action<Color>, startValue as Func<Color>, targetValue as Func<Color>, duration, delay, callback, scaledTime, tweenType);
            }
            else
            {
                Debug.LogWarning("Value type not supported for tweening");
                return 0;
            }
        }

        public static int TweenTweenValueCustom<T>(Action<T> updateValue, T startValue, T targetValue, float duration, AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenValueCustom<T>(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenValueCustom<T>(Action<T> updateValue, Func<T> startValue, T targetValue, float duration, AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenValueCustom<T>(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenValueCustom<T>(Action<T> updateValue, Func<T> startValue, Func<T> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
            if (typeof(T) == typeof(float))
            {
                return TweenFloatCustom(updateValue as Action<float>, startValue as Func<float>, targetValue as Func<float>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(int))
            {
                return TweenIntCustom(updateValue as Action<int>, startValue as Func<int>, targetValue as Func<int>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(Vector2))
            {
                return TweenVector2Custom(updateValue as Action<Vector2>, startValue as Func<Vector2>, targetValue as Func<Vector2>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(Vector3))
            {
                return TweenVector3Custom(updateValue as Action<Vector3>, startValue as Func<Vector3>, targetValue as Func<Vector3>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(Vector4))
            {
                return TweenVector4Custom(updateValue as Action<Vector4>, startValue as Func<Vector4>, targetValue as Func<Vector4>, duration, animationCurve, delay, callback, scaledTime);
            }
            else if (typeof(T) == typeof(Color))
            {
                return TweenColorCustom(updateValue as Action<Color>, startValue as Func<Color>, targetValue as Func<Color>, duration, animationCurve, delay, callback, scaledTime);
            }
            else
            {
                Debug.LogWarning("Value type not supported for tweening");
                return 0;
            }
        }

        #endregion

        #region float

        [SerializeField]
        private TweenQueue<AutoTweenFloat> m_TweenFloatQueue = new TweenQueue<AutoTweenFloat>();

        public static int TweenFloat(Action<float> updateValue, float startValue, float targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenFloat(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime, tweenType);
        }

        public static int TweenFloat(Action<float> updateValue, Func<float> startValue, float targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenFloat(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime, tweenType);
        }

        public static int TweenFloat(Action<float> updateValue, Func<float> startValue, Func<float> targetValue, float duration, float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenFloat tween = instance.m_TweenFloatQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenFloatCustom(Action<float> updateValue, float startValue, float targetValue, float duration, AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenFloatCustom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenFloatCustom(Action<float> updateValue, Func<float> startValue, float targetValue, float duration, AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenFloatCustom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenFloatCustom(Action<float> updateValue, Func<float> startValue, Func<float> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenFloat tween = instance.m_TweenFloatQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, Tween.TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region int

        [SerializeField]
        private TweenQueue<AutoTweenInt> m_TweenIntQueue = new TweenQueue<AutoTweenInt>();

        public static int TweenInt(Action<int> updateValue, int startValue, int targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenInt(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenInt(Action<int> updateValue, Func<int> startValue, int targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenInt(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenInt(Action<int> updateValue, Func<int> startValue, Func<int> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenInt tween = instance.m_TweenIntQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenIntCustom(Action<int> updateValue, int startValue, int targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenIntCustom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenIntCustom(Action<int> updateValue, Func<int> startValue, int targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenIntCustom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenIntCustom(Action<int> updateValue, Func<int> startValue, Func<int> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenInt tween = instance.m_TweenIntQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, Tween.TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region Vector2

        [SerializeField]
        private TweenQueue<AutoTweenVector2> m_TweenVector2Queue = new TweenQueue<AutoTweenVector2>();

        public static int TweenVector2(Action<Vector2> updateValue, Vector2 startValue, Vector2 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenVector2(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector2(Action<Vector2> updateValue, Func<Vector2> startValue, Vector2 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenVector2(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector2(Action<Vector2> updateValue, Func<Vector2> startValue, Func<Vector2> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenVector2 tween = instance.m_TweenVector2Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenVector2Custom(Action<Vector2> updateValue, Vector2 startValue, Vector2 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector2Custom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector2Custom(Action<Vector2> updateValue, Func<Vector2> startValue, Vector2 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector2Custom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector2Custom(Action<Vector2> updateValue, Func<Vector2> startValue, Func<Vector2> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenVector2 tween = instance.m_TweenVector2Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, Tween.TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region Vector3

        [SerializeField]
        private TweenQueue<AutoTweenVector3> m_TweenVector3Queue = new TweenQueue<AutoTweenVector3>();

        public static int TweenVector3(Action<Vector3> updateValue, Vector3 startValue, Vector3 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenVector3(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector3(Action<Vector3> updateValue, Func<Vector3> startValue, Vector3 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenVector3(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector3(Action<Vector3> updateValue, Func<Vector3> startValue, Func<Vector3> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenVector3 tween = instance.m_TweenVector3Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenVector3Custom(Action<Vector3> updateValue, Vector3 startValue, Vector3 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector3Custom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector3Custom(Action<Vector3> updateValue, Func<Vector3> startValue, Vector3 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector3Custom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector3Custom(Action<Vector3> updateValue, Func<Vector3> startValue, Func<Vector3> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenVector3 tween = instance.m_TweenVector3Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, Tween.TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region Vector4

        [SerializeField]
        private TweenQueue<AutoTweenVector4> m_TweenVector4Queue = new TweenQueue<AutoTweenVector4>();

        public static int TweenVector4(Action<Vector4> updateValue, Vector4 startValue, Vector4 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenVector4(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector4(Action<Vector4> updateValue, Func<Vector4> startValue, Vector4 targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenVector4(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenVector4(Action<Vector4> updateValue, Func<Vector4> startValue, Func<Vector4> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenVector4 tween = instance.m_TweenVector4Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenVector4Custom(Action<Vector4> updateValue, Vector4 startValue, Vector4 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector4Custom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector4Custom(Action<Vector4> updateValue, Func<Vector4> startValue, Vector4 targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenVector4Custom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenVector4Custom(Action<Vector4> updateValue, Func<Vector4> startValue, Func<Vector4> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenVector4 tween = instance.m_TweenVector4Queue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, Tween.TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion

        #region Color

        [SerializeField]
        private TweenQueue<AutoTweenColor> m_TweenColorQueue = new TweenQueue<AutoTweenColor>();

        public static int TweenColor(Action<Color> updateValue, Color startValue, Color targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenColor(updateValue, () => startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenColor(Action<Color> updateValue, Func<Color> startValue, Color targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
            return TweenColor(updateValue, startValue, () => targetValue, duration, delay, callback, scaledTime,
                tweenType);
        }

        public static int TweenColor(Action<Color> updateValue, Func<Color> startValue, Func<Color> targetValue, float duration,
            float delay = 0f, Action callback = null, bool scaledTime = false, Tween.TweenType tweenType = Tween.TweenType.EaseOutQuint)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenColor tween = instance.m_TweenColorQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, tweenType, callback, null, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        public static int TweenColorCustom(Action<Color> updateValue, Color startValue, Color targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenColorCustom(updateValue, () => startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenColorCustom(Action<Color> updateValue, Func<Color> startValue, Color targetValue, float duration,
            AnimationCurve animationCurve, float delay = 0f, Action callback = null, bool scaledTime = false)
        {
            return TweenColorCustom(updateValue, startValue, () => targetValue, duration, animationCurve, delay, callback, scaledTime);
        }

        public static int TweenColorCustom(Action<Color> updateValue, Func<Color> startValue, Func<Color> targetValue, float duration, AnimationCurve animationCurve, float delay = 0, Action callback = null, bool scaledTime = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return -2;
            }
#endif
            AutoTweenColor tween = instance.m_TweenColorQueue.GetTween();

            int id = instance.m_TweenIdCount;
            instance.m_TweenIdCount++;

            tween.Initialize(updateValue, startValue, targetValue, duration, delay, Tween.TweenType.Custom, callback, animationCurve, scaledTime, id);

            instance.m_ActiveTweens.Add(tween);

            return id;
        }

        #endregion
    }
}