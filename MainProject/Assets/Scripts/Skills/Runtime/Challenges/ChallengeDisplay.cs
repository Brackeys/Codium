// *************************************
// The ChallengeDisplay class registers methods on delegates from the ChallengeManager
// in order to make the scene react to what is happening with the challenges.
// All animation and UI code regarding challenges should recide here.
// *************************************

using UnityEngine;
using ProgressBar;
using TMPro;
using MaterialUI;

namespace Codium.Challenges
{
	public class ChallengeDisplay : MonoBehaviour {

		// ***** UI REFERENCES *****
		
		[SerializeField]
		private TextMeshProUGUI m_missionText;

		[SerializeField]
		private TextMeshProUGUI m_answerText;
		
		[SerializeField]
		private ProgressBarBehaviour m_progressSlider;
		
		[SerializeField]
		private MaterialButton m_checkAnswerButton;

		[Header("Animation")]
		[SerializeField]
		Animator m_footerAnim;
		[SerializeField]
		string m_correctAnswerTrigger = "CorrectAnswer";
		[SerializeField]
		string m_wrongAnswerTrigger = "WrongAnswer";
		[SerializeField]
		string m_resetFooterTrigger = "Reset";
		
		//Caching
		ChallengeManager m_challengeManager;
		
		void Awake () {
			//Error checking
			if (m_missionText == null)
				Debug.LogError("No missionText!");
			if (m_answerText == null)
				Debug.LogError("No answerText!");
			if (m_progressSlider == null) 
				Debug.LogError ("No progressSlider referenced.");
			if (m_checkAnswerButton == null)
				Debug.LogError ("No CheckAnswerButton referenced.");
			if (m_footerAnim == null)
				Debug.LogError ("No footerAnim referenced.");
			
			//Caching
			m_challengeManager = ChallengeManager.Instance;
			if (m_challengeManager == null)
				Debug.LogError("No ChallengeManager found!");

			//Callbacks
			m_challengeManager.onResetChallenge += OnResetChallenge;
			m_challengeManager.onCompleteChallenge += OnCompleteChallenge;
			m_challengeManager.onCorrectAnswer += OnCorrectAnswer;
			m_challengeManager.onWrongAnswer += OnWrongAnswer;
			m_challengeManager.onBeginChallenge += OnBeginChallenge;
			m_challengeManager.onAnswerSelected += OnAnswerSelected;
		}
		
		//When a challenge is completed
		void OnCompleteChallenge (float completionPercentage) {
			m_progressSlider.SetFillerSizeAsPercentage( completionPercentage );
		}
		
		//When a challenge is reset
		void OnResetChallenge () {
			m_footerAnim.SetTrigger(m_resetFooterTrigger);
			m_checkAnswerButton.interactable = false;
		}
		
		//When a challenge is answered correctly
		void OnCorrectAnswer () {
			m_footerAnim.SetTrigger(m_correctAnswerTrigger);
		}
		
		//When a challenge is answered wrong
		void OnWrongAnswer (string correctAnswer) {
			m_footerAnim.SetTrigger(m_wrongAnswerTrigger);
			
			m_answerText.text = correctAnswer;
		}
		
		
		//When a new challenge is begun
		void OnBeginChallenge (ChallengeData challenge) {
			m_missionText.text = challenge.mission;
		}
		
		//When a answer is selected
		void OnAnswerSelected () {
			if (!m_checkAnswerButton.interactable)
				m_checkAnswerButton.interactable = true;
		}

	}	
}