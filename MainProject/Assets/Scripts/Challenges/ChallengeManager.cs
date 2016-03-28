using UnityEngine;
using MaterialUI;

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

		private bool m_challengeComplete = false;
		public bool challengeComplete { get { return m_challengeComplete; } }

		void Awake ()
		{
			if (m_quizChallenge == null)
				Debug.LogError("No QuizChallenge referenced!");

			if (footerAnim == null)
				Debug.LogError("No footer animator referenced!");

			if (checkAnswerButton == null)
				Debug.LogError("No checkAnswerButton referenced!");
		}

		void Start ()
		{
			m_quizChallenge.gameObject.SetActive(true);
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
			Debug.Log("Continuing to next challenge!");
			ResetChallenge();
		}

		void ResetChallenge ()
		{
			footerAnim.SetTrigger(m_resetFooterTrigger);
			UncompleteChallenge();
		}

		public void EnableCheckAnswerButton (bool state)
		{
			checkAnswerButton.interactable = state;
        }

	}

}
