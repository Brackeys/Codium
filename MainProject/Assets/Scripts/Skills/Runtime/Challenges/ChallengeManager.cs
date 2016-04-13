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
		FITBChallenge m_fitbChallenge;

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

		//Callbacks
		public delegate void ResetChallengeCallback();
		public ResetChallengeCallback onResetChallenge;
		public delegate void CheckAnswerCallback();
		public CheckAnswerCallback onCheckAnswer;

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
				case ChallengeType.FITB:
					m_fitbChallenge.gameObject.SetActive(true);
					m_fitbChallenge.InitChallenge(_challenge);
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

			footerAnim.SetTrigger(m_correctAnswerTrigger);
		}

		public void WrongAnswer()
		{
			CompleteChallenge();

			footerAnim.SetTrigger(m_wrongAnswerTrigger);
		}

		public void CheckAnswer ()
		{
			onCheckAnswer.Invoke();
		}

		public void OnSkipChallenge()
		{
			ContinueToNextChallenge();
		}

		public void OnContinueChallenge()
		{
			ContinueToNextChallenge();
		}

		private void ContinueToNextChallenge ()
		{
			onResetChallenge.Invoke();
			ResetChallengeOnManager();

			m_currentChallengeIndex += 1;
			BeginChallenge(m_currentChallengeIndex);
		}

		private void ResetChallengeOnManager ()
		{
			footerAnim.SetTrigger(m_resetFooterTrigger);
			UncompleteChallenge();

			//*****
			m_quizChallenge.gameObject.SetActive(false);
			m_fitbChallenge.gameObject.SetActive(false);
		}

		public void EnableCheckAnswerButton (bool state)
		{
			checkAnswerButton.interactable = state;
        }

	}

}
