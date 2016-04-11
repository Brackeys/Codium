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

			string _fillCode = _fitbData.fillCode;

			if (!_fillCode.Contains(BLANK_TAG))
			{
				Debug.LogError("No " + BLANK_TAG + " found.");
				return;
			}

			m_codeBefore.text = _fillCode.Substring(0, _fillCode.IndexOf(BLANK_TAG));
			m_dropdown.SetOptions(_fitbData.fillOne, _fitbData.fillTwo, _fitbData.fillThree);
			m_codeAfter.text = _fillCode.Substring(_fillCode.IndexOf(BLANK_TAG) + BLANK_TAG.Length);
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
				base.WrongAnswer(m_challengeData.fitbChallengeData.GetCorrectAnswer());
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
