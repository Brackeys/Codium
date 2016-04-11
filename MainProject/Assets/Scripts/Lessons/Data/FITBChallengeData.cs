using UnityEngine;

namespace Codium.Challenges
{
	public enum FITBAnswer {ONE, TWO, THREE};
	
	[System.Serializable]
	public class FITBChallengeData {
		
		// The code with a blank element
		public string fillCode;
		
		//The three options
		public string fillOne;
		public string fillTwo;
		public string fillThree;
		
		public FITBAnswer answer;

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
