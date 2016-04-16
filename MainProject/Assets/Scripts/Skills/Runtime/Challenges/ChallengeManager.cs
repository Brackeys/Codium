// *************************************
// This class manages all challenges.
// This includes what happens on challenge skips, correct/wrong answers
// and when to transition to new challenges.
// The ChallengeManager is also responsible for adminestering loading in different challenge-types.
// *************************************

using UnityEngine;

namespace Codium.Challenges
{
	public class ChallengeManager : Singleton<ChallengeManager>
	{

		protected ChallengeManager() {}

		//References to objects containing the different challenge-types.
		[SerializeField]
		QuizChallenge m_quizChallenge;
		[SerializeField]
		FITBChallenge m_fitbChallenge;

		//TODO: Better way of keeping track of current challenges
		private int m_currentChallengeIndex;

		//Callbacks
		public delegate void ResetChallengeCallback();
		public ResetChallengeCallback onResetChallenge;
		public delegate void CheckAnswerCallback();
		public CheckAnswerCallback onCheckAnswer;
		public delegate void CompleteChallengeCallback(float completionPercentage);
		public CompleteChallengeCallback onCompleteChallenge;
		public delegate void CorrectAnswerCallback();
		public CorrectAnswerCallback onCorrectAnswer;
		public delegate void WrongAnswerCallback(string correctAnswer);
		public WrongAnswerCallback onWrongAnswer;
		public delegate void AnswerSelectedCallback();
		public AnswerSelectedCallback onAnswerSelected;
		public delegate void OnBeginChallengeCallback (ChallengeData challenge);
		public OnBeginChallengeCallback onBeginChallenge;

		//Caching
		LessonManager m_lessonManager;

		void Awake () {
			//Caching
			m_lessonManager = LessonManager.Instance;
			if (m_lessonManager == null) {
				Debug.LogError ("No LessonManager found!");
			}
		}

		void Start ()
		{
			//Begin by resetting the current challenge
			ResetChallenge();

			//TODO: Load in a certain challenge, not just the first in the list
			m_currentChallengeIndex = 0;
			BeginChallenge();
		}
		
		private ChallengeData GetCurrentChallenge () {
			//TODO: This can be deleted when a proper way of keeping track
			//of the current challenge is implemented.
			
			LessonData _lesson = LessonManager.Instance.CurrentLesson;
			if (m_currentChallengeIndex >= _lesson.challenges.Length)
			{
				Debug.LogError("Challenge index out of bounds: " + m_currentChallengeIndex);
				return null;
			}
			
			return _lesson.challenges[m_currentChallengeIndex];
		}

		//Begin the current challenge
		void BeginChallenge ()
		{
			ChallengeData _challenge = GetCurrentChallenge();
			
			//Invoke onBeginChallenge
			onBeginChallenge.Invoke(_challenge);

			//Load and init challenge based on type
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

		//Complete the current challenge
		//A challenge is currently completed no matter if the answer was correct or not
		void CompleteChallenge ()
		{
			LessonData _lesson = m_lessonManager.CurrentLesson;
			float _completionPercentage = ((float)(m_currentChallengeIndex + 1)/_lesson.challenges.Length) * 100f;
			
			if (onCompleteChallenge != null)
				onCompleteChallenge.Invoke(_completionPercentage);
		}

		//This method is called when a correct answer is chosen
		//It is called by the current Challenge
		public void CorrectAnswer ()
		{
			if (onCorrectAnswer != null)
				onCorrectAnswer.Invoke();
			
			CompleteChallenge();
		}

		//This method is called when a wrong answer is chosen
		//It is called by the current Challenge
		public void WrongAnswer(string correctAnswer)
		{
			if (onWrongAnswer != null)
				onWrongAnswer.Invoke(correctAnswer);
			
			CompleteChallenge();
		}
		
		//Continue to the next challenge
		//TODO: Chose the next challenge by criteria, not just the next in the list
		private void ContinueToNextChallenge ()
		{
			ResetChallenge();

			m_currentChallengeIndex += 1;
			BeginChallenge();
		}

		//Reset the current challenge
		//Unload any potentialy loaded challenges
		private void ResetChallenge ()
		{
			if (onResetChallenge != null)
				onResetChallenge.Invoke();

			m_quizChallenge.gameObject.SetActive(false);
			m_fitbChallenge.gameObject.SetActive(false);
		}
		
		// ***** BUTTON METHOD CALLS *****
		
		public void CheckAnswer_Button ()
		{
			if (onCheckAnswer != null)
				onCheckAnswer.Invoke();
		}

		public void SkipChallenge_Button()
		{
			ContinueToNextChallenge();
		}

		public void ContinueChallenge_Button()
		{
			ContinueToNextChallenge();
		}

	}

}
