//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

// From: https://gist.github.com/benblo/10732554

#if UNITY_EDITOR
using System.Collections;
using UnityEditor;

namespace MaterialUI
{
	public class EditorCoroutine
	{
		public static EditorCoroutine Start(IEnumerator routine)
		{
			EditorCoroutine coroutine = new EditorCoroutine(routine);
			coroutine.Start();
			return coroutine;
		}
		
		private readonly IEnumerator m_Routine;
		
		EditorCoroutine(IEnumerator routine)
		{
			this.m_Routine = routine;
		}
		
		void Start()
		{
			EditorApplication.update += Update;
		}
		
		void Stop()
		{
			EditorApplication.update -= Update;
		}
		
		void Update()
		{
			if (!m_Routine.MoveNext())
			{
				Stop();
			}
		}
	}
}
#endif