using UnityEngine;
using UnityEngine.UI;

namespace Codium.Challenges {

	public abstract class Challenge : MonoBehaviour
	{
		//Data for the challenge
		protected ChallengeData m_challengeData;

		//UI References
		[SerializeField]
		protected Text m_missionText;
		
		//Caching
		ChallengeManager m_challengeManager;
		protected ChallengeManager challengeManager { get {return m_challengeManager;} }

		//Has an answer been chosen?
		private bool m_answerSelected = false;

		void Awake ()
		{
			m_challengeManager = ChallengeManager.Instance;
			if (m_challengeManager == null)
				Debug.LogError("No ChallengeManager found!");
		}

		abstract public void CheckAnswer();
		abstract public void InitChallenge(ChallengeData challenge);

		protected void OnAnswerSelected ()
		{
			if (!m_answerSelected)
			{
				m_answerSelected = true;
				m_challengeManager.EnableCheckAnswerButton(true);
            }
		}

		protected void CorrectAnswer()
		{
			m_challengeManager.CorrectAnswer();
		}

		protected void WrongAnswer()
		{
			m_challengeManager.WrongAnswer();
		}

		public void OnSkipChallenge ()
		{
			m_challengeManager.OnSkipChallenge();
			ResetChallenge();
		}

		public void OnContinueChallenge()
		{
			m_challengeManager.OnContinueChallenge();
			ResetChallenge();
		}

		virtual protected void ResetChallenge ()
		{
			m_answerSelected = false;
			m_challengeManager.EnableCheckAnswerButton(false);
		}

	}

}
