// *************************************
// Data class for storing information about a challenge.
// *************************************

using UnityEngine;

namespace Codium.Challenges
{
	[System.Serializable]
	public class ChallengeData {
		
		public ChallengeData () {
			
		}

		//The type of challenge (enum)
		public ChallengeType type;
		
		//The mission of the challenge
		public string mission;
		
		//These are challenge data objects for challenge-type-specific information
		//These will be ignored unless they match the challenge-type
		public QuizChallengeData quizChallengeData;
		public FITBChallengeData fitbChallengeData;
		
		//Utility method for getting the correct answer as a string
		public string GetCorrectAnswer () {
			switch (type)
			{
				case ChallengeType.QUIZ:
					return quizChallengeData.GetCorrectAnswer();
				case ChallengeType.FITB:
					return fitbChallengeData.GetCorrectAnswer();
				default:
					Debug.LogError("No case for " + type.ToString() + " implemented.");
					return "ERROR";
			}
		}
		
	}

	//Challenge type enum
	public enum ChallengeType
	{
		QUIZ, FITB
	}
	
}