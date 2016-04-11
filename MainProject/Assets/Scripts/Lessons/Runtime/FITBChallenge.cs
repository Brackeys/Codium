using UnityEngine;
using TMPro;
using Codium.UI;
using System;

namespace Codium.Challenges
{

	public class FITBChallenge : Challenge
	{
		
		private const string BLANK_TAG = "*BLANK*";

		[SerializeField]
		private TextMeshProUGUI m_codeBefore;
		[SerializeField]
		private TextMeshProUGUI m_codeAfter;
		[SerializeField]
		private DropdownMenu_ThreeOptions m_dropdown;

		private FITBAnswer m_selectedAnswer;

		public override void InitChallenge(ChallengeData challenge)
		{
			base.InitChallenge(challenge);

			FITBChallengeData _fitbData = m_challengeData.fitbChallengeData;

			string[] result;
			
			result = _fitbData.fillCode.Split(BLANK_TAG.ToCharArray(), StringSplitOptions.None);

			Debug.Log ("Elements: " + result.Length);
			
			for (int i = 0; i < result.Length; i++)
			{
				Debug.Log (result[i]);
			}

			m_codeBefore.text = "test ";
			m_dropdown.SetOptions(_fitbData.fillOne, _fitbData.fillTwo, _fitbData.fillThree);
			m_codeAfter.text = " og mere test";
		}

		override protected void CheckAnswer()
		{
			if (!gameObject.activeSelf)
				return;

			if (m_selectedAnswer == m_challengeData.fitbChallengeData.answer)
			{
				base.CorrectAnswer();
			}
			else
			{
				base.WrongAnswer();
			}
		}

		override protected void ResetChallenge()
		{
			base.ResetChallenge();
		}

		public void SelectAnswer(int answer)
		{
			if (challengeManager.challengeComplete)
				return;

			m_selectedAnswer = (FITBAnswer)(answer);

			OnAnswerSelected();
		}

	}

}
