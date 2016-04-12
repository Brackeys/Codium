using UnityEngine;
using MaterialUI;
using Codium.UI;
using System;

namespace Codium.Challenges {

	public class QuizChallenge : Challenge
	{

		[SerializeField]
		private MaterialButton m_buttonOne;
		[SerializeField]
		private MaterialButton m_buttonTwo;
		[SerializeField]
		private MaterialButton m_buttonThree;

		[SerializeField]
		private string m_selectColorName;

		private Color m_startColor;

		private QuizAnswer m_selectedAnswer;

		void Start()
		{
			m_startColor = m_buttonOne.backgroundImage.color;
		}

		public override void InitChallenge(ChallengeData challenge)
		{
			base.InitChallenge(challenge);

			QuizChallengeData _quizData = m_challengeData.quizChallengeData;
			m_buttonOne.text.text = _quizData.optionOne;
			m_buttonTwo.text.text = _quizData.optionTwo;
			m_buttonThree.text.text = _quizData.optionThree;
		}

		override protected void CheckAnswer()
		{
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

		override protected void ResetChallenge ()
		{
			base.ResetChallenge();

			m_buttonOne.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonTwo.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonThree.materialRipple.SetGraphicColor(m_startColor, true);
		}

		public void SelectAnswerOne()
		{
			if (challengeManager.challengeComplete)
				return;

			m_selectedAnswer = QuizAnswer.ONE;

			OnAnswerSelected();

			m_buttonOne.materialRipple.SetGraphicColor(ColorManager.GetColor(m_selectColorName), true);
			m_buttonTwo.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonThree.materialRipple.SetGraphicColor(m_startColor, true);
		}
		public void SelectAnswerTwo()
		{
			if (challengeManager.challengeComplete)
				return;

			m_selectedAnswer = QuizAnswer.TWO;

			OnAnswerSelected();

			m_buttonOne.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonTwo.materialRipple.SetGraphicColor(ColorManager.GetColor(m_selectColorName), true);
			m_buttonThree.materialRipple.SetGraphicColor(m_startColor, true);
		}
		public void SelectAnswerThree()
		{
			if (challengeManager.challengeComplete)
				return;

			m_selectedAnswer = QuizAnswer.THREE;

			OnAnswerSelected();

			m_buttonOne.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonTwo.materialRipple.SetGraphicColor(m_startColor, true);
			m_buttonThree.materialRipple.SetGraphicColor(ColorManager.GetColor(m_selectColorName), true);
		}

	}

}
