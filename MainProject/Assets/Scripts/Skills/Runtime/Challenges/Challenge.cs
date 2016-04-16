// *************************************
// The Challenge class acts as a base for all challenges.
// It uses a ChallengeData object to load all information about the currect challenge.
// It is managed by the ChallengeManager.
// *************************************

using UnityEngine;

namespace Codium.Challenges {

	public abstract class Challenge : MonoBehaviour
	{
		//Data for the challenge
		protected ChallengeData m_challengeData;
		
		//Caching
		private ChallengeManager m_challengeManager;
		protected ChallengeManager challengeManager { get {return m_challengeManager;} }

		void Awake ()
		{
			//Caching
			m_challengeManager = ChallengeManager.Instance;
			if (m_challengeManager == null)
				Debug.LogError("No ChallengeManager found!");

			//Callbacks
			m_challengeManager.onResetChallenge += ResetChallenge;
			m_challengeManager.onCheckAnswer += CheckAnswer;
		}

		//Abstact methods
		abstract protected void CheckAnswer();
		abstract protected void ResetChallenge ();
		
		//Set up the challenge by loading in the ChallengeData
		virtual public void InitChallenge(ChallengeData challenge)
		{
			m_challengeData = challenge;
		}

		//Called when an answer is selected
		protected void AnswerSelected ()
		{	
			//Invoke the onAnswerSelected delegate on the ChallengeManager
			//This will make sure that the Unity scene reacts to a answer being chosen.
			m_challengeManager.onAnswerSelected.Invoke();
		}

		//Called by derived classes when a correct answer has been selected
		//AND the user has pressed the "Check" button.
		protected void CorrectAnswer()
		{
			//Call CorrectAnswer on the ChallengeManager.
			m_challengeManager.CorrectAnswer();
		}

		//Called by derived classes when a wrong answer has been selected
		//AND the user has pressed the "Check" button
		//The correct answer is provided as an argument (string)
		protected void WrongAnswer(string correctAnswer)
		{
			//Call WrongAnswer on the ChallengeManager
			m_challengeManager.WrongAnswer(correctAnswer);
		}

	}

}
