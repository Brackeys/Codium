// *************************************
// Data for the challenge-type "Quiz".
// *************************************

using UnityEngine;

namespace Codium.Challenges
{
	//Quiz answer enum
	public enum QuizAnswer {ONE, TWO, THREE};
	
	[System.Serializable]
	public class QuizChallengeData {
		
		//The three options to choose between
		public string optionOne;
		public string optionTwo;
		public string optionThree;
		
		//The correct answer
		public QuizAnswer answer;

		//Utility method for getting the correct answer as a string
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
