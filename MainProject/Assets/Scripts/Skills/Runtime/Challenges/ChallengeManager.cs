// *************************************
// This class manages all challenges.
// This includes what happens on challenge skips, correct/wrong answers
// and when to transition to new challenges.
// The ChallengeManager is also responsible for adminestering loading in different challenge-types.
// *************************************

using UnityEngine;

namespace Codium.Challenges
{
	[RequireComponent(typeof(ChallengeProgressManager))]
	public class ChallengeManager : Singleton<ChallengeManager>
	{

		protected ChallengeManager() {}

		//References to objects containing the different challenge-types.
		[SerializeField]
		QuizChallenge m_quizChallenge;
		[SerializeField]
		FITBChallenge m_fitbChallenge;

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
		
		//Properties
		private ChallengeData CurrentChallenge { get { return m_progressManager.CurrentChallenge; } }

		//Caching
		LessonManager m_lessonManager;
		ChallengeProgressManager m_progressManager;

		void Awake () {
			//Caching
			m_progressManager = GetComponent<ChallengeProgressManager>();
		}

		void Start ()
		{
			ContinueToNextChallenge();
		}

		//Begin the current challenge
		private void BeginChallenge ()
		{	
			//Invoke onBeginChallenge
			onBeginChallenge.Invoke(CurrentChallenge);

			//Load and init challenge based on type
			switch (CurrentChallenge.type)
			{
				case ChallengeType.QUIZ:
					m_quizChallenge.gameObject.SetActive(true);
					m_quizChallenge.InitChallenge(CurrentChallenge);
					break;
				case ChallengeType.FITB:
					m_fitbChallenge.gameObject.SetActive(true);
					m_fitbChallenge.InitChallenge(CurrentChallenge);
					break;
				default:
					Debug.LogError("No such challenge type registered here: " + CurrentChallenge.type);
					return;
			}
		}

		//Complete the current challenge
		private void CompleteChallenge ()
		{
			//Complete challenge in progress manager
			m_progressManager.CompleteChallenge();
			
			//Get the completionPercentage
			float _completionPercentage = m_progressManager.GetCompletionPercentage();
			
			//Invoke the onCompleteChallenge callback
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
		}
		
		//Continue to the next challenge
		private void ContinueToNextChallenge ()
		{
			//Reset the current challenge
			ResetChallenge();

			//Progress to the next challenge
			m_progressManager.ProgressToNextChallenge();
			
			//Begin the challenge
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
