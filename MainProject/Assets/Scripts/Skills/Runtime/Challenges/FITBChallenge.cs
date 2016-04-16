// *************************************
// FITBChallenge derives from Challenge.
// It implements functionality specific to "fill in the blank" challenges.
// *************************************

using UnityEngine;
using TMPro;
using Codium.UI;

namespace Codium.Challenges
{

	public class FITBChallenge : Challenge
	{
		//The tag to check for when inserting a blank dropdown
		private const string BLANK_TAG = "*BLANK*";

		// ***** UI REFERENCES *****

		//TextMeshProUGUI object to show the code before the blank tag
		[SerializeField]
		private TextMeshProUGUI m_codeBefore;
		//TextMeshProUGUI object to show the code before the blank tag
		[SerializeField]
		private TextMeshProUGUI m_codeAfter;
		
		//DropdownMenu to insert at the blank spot
		[SerializeField]
		private DropdownMenu_ThreeOptions m_dropdown;

		//Currently selected answer (enum)
		private FITBAnswer m_selectedAnswer;

		//Set up the challenge using ChallengeData
		public override void InitChallenge(ChallengeData challenge)
		{
			//Call the base InitChallenge method
			base.InitChallenge(challenge);

			//Get the FITB data
			FITBChallengeData _fitbData = m_challengeData.fitbChallengeData;

			//Set the code objects and the dropdown menu with the correct options

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

		//Check the current answer
		//Call base methods depending on wether or not it is correct
		override protected void CheckAnswer()
		{
			//Only do the check if the object is currently active
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

		//Reset the challenge
		//No code needs to be implemented here if the challenge doesn't require it
		override protected void ResetChallenge()
		{
			//Do nothing so far
		}

		//Select a certain answer
		public void SelectAnswer(int answer)
		{
			m_selectedAnswer = (FITBAnswer)(answer);

			base.AnswerSelected();
		}

	}

}
