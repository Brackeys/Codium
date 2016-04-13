using UnityEngine;

namespace Codium.Challenges
{
	public enum QuizAnswer {ONE, TWO, THREE};
	
	[System.Serializable]
	public class QuizChallengeData {
		
		public string optionOne;
		public string optionTwo;
		public string optionThree;
		
		public QuizAnswer answer;

		public string GetCorrectAnswer ()
		{
			switch (answer)
			{
				case QuizAnswer.ONE:
					return optionOne;
				case QuizAnswer.TWO:
					return optionTwo;
				case QuizAnswer.THREE:
					return optionThree;
				default:
					Debug.LogError("No case for " + answer.ToString() + " implemented.");
					return "ERROR";
			}
		}
		
	}
	
}
