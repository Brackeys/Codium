using UnityEngine;
using UnityEngine.UI;

namespace Codium
{
	public class LessonManager : Singleton<LessonManager> {

		protected LessonManager() { }

		//TEMP: Until lesson selection system is implemented
		[SerializeField]
		private LessonData startLesson;

		[SerializeField]
		private Slider m_progressSlider;

		private LessonData m_currentLesson;
		public LessonData CurrentLesson { get { return m_currentLesson; } }

		void Awake ()
		{
			SetLesson(startLesson);
		}

		void SetLesson (LessonData lesson)
		{
			m_currentLesson = lesson;

			m_progressSlider.maxValue = m_currentLesson.challenges.Length;
		}

	}
}
