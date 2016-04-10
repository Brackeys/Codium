using UnityEngine;
using TMPro;

namespace Codium.Challenges {

	public abstract class Challenge : MonoBehaviour
	{
		//Data for the challenge
		protected ChallengeData m_challengeData;

		//UI References
		[SerializeField]
		private TextMeshProUGUI m_missionText;
		
		//Caching
		private ChallengeManager m_challengeManager;
		protected ChallengeManager challengeManager { get {return m_challengeManager;} }

		//Has an answer been chosen?
		private bool m_answerSelected = false;

		void Awake ()
		{
			m_challengeManager = ChallengeManager.Instance;
			if (m_challengeManager == null)
				Debug.LogError("No ChallengeManager found!");

			m_challengeManager.onResetChallenge += ResetChallenge;
			m_challengeManager.onCheckAnswer += CheckAnswer;
		}

		abstract protected void CheckAnswer();
		virtual public void InitChallenge(ChallengeData challenge)
		{
			m_challengeData = challenge;
			m_missionText.text = challenge.mission;
		}

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

		virtual protected void ResetChallenge ()
		{
			m_answerSelected = false;
			m_challengeManager.EnableCheckAnswerButton(false);
		}

	}

}
