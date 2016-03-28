using UnityEngine;
using MaterialUI;

namespace Codium.Challenges {

	public class QuizChallenge : Challenge
	{

		public enum QuizAnswer { ONE, TWO, THREE };
		[SerializeField]
		private QuizAnswer m_correctAnswer;

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

		override public void CheckAnswer()
		{
			if (!gameObject.activeSelf)
				return;

			if (m_selectedAnswer == m_correctAnswer)
			{
				base.CorrectAnswer();
			}
			else
			{
				base.WrongAnswer();
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
