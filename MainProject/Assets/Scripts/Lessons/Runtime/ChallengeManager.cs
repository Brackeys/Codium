using UnityEngine;
using MaterialUI;
using UnityEngine.UI;

namespace Codium.Challenges
{
	public class ChallengeManager : Singleton<ChallengeManager>
	{

		protected ChallengeManager() {}

		[SerializeField]
		QuizChallenge m_quizChallenge;

		[SerializeField]
		MaterialButton checkAnswerButton;

		[Header("Animation")]
		[SerializeField]
		Animator footerAnim;
		[SerializeField]
		string m_correctAnswerTrigger = "CorrectAnswer";
		[SerializeField]
		string m_wrongAnswerTrigger = "WrongAnswer";
		[SerializeField]
		string m_resetFooterTrigger = "Reset";

		[SerializeField]
		private Slider m_progressSlider;

		private bool m_challengeComplete = false;
		public bool challengeComplete { get { return m_challengeComplete; } }

		private int m_currentChallengeIndex;

		// Caching
		private LessonManager m_lessonManager;

		void Start ()
		{
			m_lessonManager = LessonManager.Instance;

			m_currentChallengeIndex = 0;
			BeginChallenge(m_currentChallengeIndex);
		}

		void BeginChallenge (int challengeIndex)
		{
			m_progressSlider.value = challengeIndex + 1;

			LessonData _lesson = m_lessonManager.CurrentLesson;
			if (challengeIndex >= _lesson.challenges.Length)
			{
				Debug.LogError("Challenge index out of bounds: " + challengeIndex);
				return;
			}

			ChallengeData _challenge = _lesson.challenges[challengeIndex];

			switch (_challenge.type)
			{
				case ChallengeType.QUIZ:
					m_quizChallenge.gameObject.SetActive(true);
					m_quizChallenge.InitChallenge(_challenge);
					break;
				default:
					Debug.LogError("No such challenge type registered here: " + _challenge.type);
					return;
			}
		}

		void CompleteChallenge ()
		{
			m_challengeComplete = true;
		}
		void UncompleteChallenge()
		{
			m_challengeComplete = false;
		}

		public void CorrectAnswer ()
		{
			CompleteChallenge();

			Debug.Log("Correct answer!");
			footerAnim.SetTrigger(m_correctAnswerTrigger);
		}

		public void WrongAnswer()
		{
			CompleteChallenge();

			Debug.Log("Wrong answer!");
			footerAnim.SetTrigger(m_wrongAnswerTrigger);
		}

		public void OnSkipChallenge()
		{
			Debug.Log("Skipping challenge!");
			ContinueToNextChallenge();
		}

		public void OnContinueChallenge ()
		{
			ContinueToNextChallenge();
		}

		public void ContinueToNextChallenge ()
		{
			ResetChallenge();

			m_currentChallengeIndex += 1;
			BeginChallenge(m_currentChallengeIndex);
		}

		void ResetChallenge ()
		{
			footerAnim.SetTrigger(m_resetFooterTrigger);
			UncompleteChallenge();

			m_quizChallenge.gameObject.SetActive(false);
		}

		public void EnableCheckAnswerButton (bool state)
		{
			checkAnswerButton.interactable = state;
        }

	}

}
