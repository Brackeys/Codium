// *************************************
// Data for the challenge-type "Fill in the blank".
// *************************************

using UnityEngine;

namespace Codium.Challenges
{
	//Answer enum
	public enum FITBAnswer {ONE, TWO, THREE};
	
	[System.Serializable]
	public class FITBChallengeData {
		
		// The code with a blank element
		// Fx. "public *BLANK* name = "John";
		public string fillCode;
		
		//The three options the user can select
		public string fillOne;
		public string fillTwo;
		public string fillThree;
		
		//The correct answer
		public FITBAnswer answer;

		// Utility method for getting the correct answer as a string
		public string GetCorrectAnswer()
		{
			switch (answer)
			{
				case FITBAnswer.ONE:
					return fillOne;
				case FITBAnswer.TWO:
					return fillTwo;
				case FITBAnswer.THREE:
					return fillThree;
				default:
					Debug.LogError("No case for " + answer.ToString() + " implemented.");
					return "ERROR";
			}
		}

	}
	
}
