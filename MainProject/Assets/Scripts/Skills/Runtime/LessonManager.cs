// *************************************
// LessonManager keeps track of what lesson to load.
// TODO: Implement this class further
// *************************************

using UnityEngine;

namespace Codium
{
	public class LessonManager : Singleton<LessonManager> {

		protected LessonManager() { }

		//TEMP: Until lesson selection system is implemented
		[SerializeField]
		private LessonData startLesson;

		private LessonData m_currentLesson;
		public LessonData CurrentLesson { get { return m_currentLesson; } }

		void Awake ()
		{
			SetLesson(startLesson);
		}

		void SetLesson (LessonData lesson)
		{
			m_currentLesson = lesson;
		}

	}
}
