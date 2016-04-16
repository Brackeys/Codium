// *************************************
// QuizChallenge derives from Challenge.
// It implements functionality specific to "quiz" challenges.
// *************************************

using UnityEngine;
using MaterialUI;
using Codium.UI;

namespace Codium.Challenges {

	public class QuizChallenge : Challenge
	{
		
		// ***** UI REFERENCES *****

		[SerializeField]
		private MaterialButton m_buttonOne;
		[SerializeField]
		private MaterialButton m_buttonTwo;
		[SerializeField]
		private MaterialButton m_buttonThree;

		//Name of the color to use for selected option
		[SerializeField]
		private string m_selectColorName;

		//Store the start color of the buttons
		private Color m_startColor;

		//Currently selected answer
		private QuizAnswer m_selectedAnswer;

		void Start()
		{
			m_startColor = m_buttonOne.backgroundImage.color;
		}

		//Setup the challenge using ChallengeData
		public override void InitChallenge(ChallengeData challenge)
		{
			//Call the base
			base.InitChallenge(challenge);

			//Setup Quiz specific objects
			QuizChallengeData _quizData = m_challengeData.quizChallengeData;
			m_buttonOne.text.text = _quizData.optionOne;
			m_buttonTwo.text.text = _quizData.optionTwo;
			m_buttonThree.text.text = _quizData.optionThree;
		}

		//Check current answer
		//Call base methods depending on wether or not the answer is correct
		override protected void CheckAnswer()
		{
			//Only check answer if gameObject is active
			if (!gameObject.activeSelf)
				return;

			if (m_selectedAnswer == m_challengeData.quizChallengeData.answer)
			{
				base.CorrectAnswer();
			}
			else
			{
				base.WrongAnswer(m_challengeData.quizChallengeData.GetCorrectAnswer());
			}
		}

		//Reset the challenge
		override protected void ResetChallenge ()
		{
			m_buttonOne.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonTwo.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonThree.materialRipple.SetGraphicColor(m_startColor, true);
		}

		// ***** METHODS FOR SELECTING AN ANSWER *****
		
		public void SelectAnswerOne()
		{
			m_selectedAnswer = QuizAnswer.ONE;

			base.AnswerSelected();

			m_buttonOne.materialRipple.SetGraphicColor(ColorManager.GetColor(m_selectColorName), true);
			m_buttonTwo.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonThree.materialRipple.SetGraphicColor(m_startColor, true);
		}
		public void SelectAnswerTwo()
		{
			m_selectedAnswer = QuizAnswer.TWO;

			base.AnswerSelected();

			m_buttonOne.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonTwo.materialRipple.SetGraphicColor(ColorManager.GetColor(m_selectColorName), true);
			m_buttonThree.materialRipple.SetGraphicColor(m_startColor, true);
		}
		public void SelectAnswerThree()
		{
			m_selectedAnswer = QuizAnswer.THREE;

			base.AnswerSelected();

			m_buttonOne.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonTwo.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonThree.materialRipple.SetGraphicColor(ColorManager.GetColor(m_selectColorName), true);
		}

	}

}
